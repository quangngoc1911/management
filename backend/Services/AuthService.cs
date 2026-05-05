using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using ManagementSystem.Configurations;
using ManagementSystem.Data;
using ManagementSystem.DTOs.Auth;
using ManagementSystem.DTOs.User;
using ManagementSystem.Entities;
using ManagementSystem.Interfaces;

namespace ManagementSystem.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;
    private readonly ApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IUserRepository userRepo,
        IJwtService jwtService,
        ApplicationDbContext context,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> LoginAsync(LoginDto request)
    {
        // Validate email format
        if (string.IsNullOrWhiteSpace(request.Email) || !System.Text.RegularExpressions.Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new UnauthorizedAccessException("Email không hợp lệ");

        // Validate password
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new UnauthorizedAccessException("Mật khẩu không được để trống");

        // 1. Tìm user
        var user = await _userRepo.GetByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng");

        // 2. Kiểm tra password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng");
        // 3. Tạo tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // 4. Lưu refresh token vào DB
        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            DeviceInfo = "web",
            CreatedAt = DateTime.UtcNow // Add CreatedAt
        });
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            }
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        // Tìm refresh token còn hiệu lực
        var tokens = await _context.RefreshTokens
            .Include(t => t.User)
            .Where(t => t.ExpiresAt > DateTime.UtcNow && !t.IsRevoked)
            .ToListAsync();

        // So khớp token gửi lên
        var stored = tokens.FirstOrDefault(t =>
            BCrypt.Net.BCrypt.Verify(refreshToken, t.TokenHash));

        if (stored == null || stored.User == null)
            throw new UnauthorizedAccessException("Refresh token không hợp lệ");

        var user = stored.User;

        // Tạo token mới
        var newAccessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        // Revoke token cũ + tạo token mới
        stored.IsRevoked = true;

        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = stored.UserId,
            TokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            }
        };
    }

    public async Task LogoutAsync(string? refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return;
        }

        var tokens = await _context.RefreshTokens
            .Where(t => !t.IsRevoked)
            .ToListAsync();

        var stored = tokens.FirstOrDefault(t =>
            BCrypt.Net.BCrypt.Verify(refreshToken, t.TokenHash));

        if (stored != null)
        {
            stored.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }
}
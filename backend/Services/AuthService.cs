using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using MyAPI.Configurations;
using MyAPI.Data;
using MyAPI.DTOs;
using MyAPI.Interfaces;
using MyAPI.Models;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;
    private readonly AppDbContext _context;
    private readonly JwtSettings _settings;

    public AuthService(
        IUserRepository userRepo,
        IJwtService jwtService,
        AppDbContext context,
        IOptions<JwtSettings> settings)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
        _context = context;
        _settings = settings.Value;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
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
            ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpiryDays),
            DeviceInfo = "web"
        });
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpiryMinutes),
            User = new UserDto { Id = user.Id, Name = user.Name, Email = user.Email }
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        // Tìm tất cả refresh token chưa hết hạn
        var tokens = await _context.RefreshTokens
            .Include(t => t.User)
            .Where(t => t.ExpiresAt > DateTime.UtcNow && !t.IsRevoked)
            .ToListAsync();

        // Tìm token khớp
        var stored = tokens.FirstOrDefault(t =>
            BCrypt.Net.BCrypt.Verify(refreshToken, t.TokenHash));

        if (stored == null)
            throw new UnauthorizedAccessException("Refresh token không hợp lệ");

        // Tạo token mới
        var newAccessToken = _jwtService.GenerateAccessToken(stored.User);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        // Xóa token cũ, thêm token mới (rotation)
        stored.IsRevoked = true;
        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = stored.UserId,
            TokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpiryDays)
        });
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpiryMinutes),
            User = new UserDto { Id = stored.User.Id, Name = stored.User.Name, Email = stored.User.Email }
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using ManagementSystem.Application.Options;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Auth.Application.DTOs;
using ManagementSystem.Modules.Users.Application.DTOs;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Application.Contracts;

namespace ManagementSystem.Modules.Auth.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwtService;
    private readonly ApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly IDateTime _dateTime;
    public AuthService(
        IUserRepository userRepo,
        IJwtService jwtService,
        ApplicationDbContext context,
        IDateTime dateTime,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
        _context = context;
        _dateTime = dateTime;
        _jwtSettings = jwtSettings.Value;
    }
    public async Task<AuthResponse> LoginAsync(LoginDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || !System.Text.RegularExpressions.Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new UnauthorizedAccessException("Email không hợp lệ");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new UnauthorizedAccessException("Mật khẩu không được để trống");

        var user = await _userRepo.GetByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Email hoặc mật khẩu không đúng");

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            TokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken),
            ExpiresAt = _dateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            DeviceInfo = "web",
            CreatedAt = _dateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = _dateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
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
        var tokens = await _context.RefreshTokens
            .Include(t => t.User)
            .Where(t => t.ExpiresAt > DateTime.UtcNow && !t.IsRevoked)
            .ToListAsync();

        var stored = tokens.FirstOrDefault(t =>
            BCrypt.Net.BCrypt.Verify(refreshToken, t.TokenHash));

        if (stored == null || stored.User == null)
            throw new UnauthorizedAccessException("Refresh token không hợp lệ");

        var user = stored.User;

        var newAccessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        stored.IsRevoked = true;

        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = stored.UserId,
            TokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken),
            ExpiresAt = _dateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            CreatedAt = _dateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiresAt = _dateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
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

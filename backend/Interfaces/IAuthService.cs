using ManagementSystem.DTOs.Auth;

namespace ManagementSystem.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginDto request);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(string? refreshToken);
}

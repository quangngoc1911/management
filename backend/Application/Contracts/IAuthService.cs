using ManagementSystem.Modules.Auth.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginDto request);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(string? refreshToken);
}
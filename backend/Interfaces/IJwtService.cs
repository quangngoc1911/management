using System.Security.Claims;

using ManagementSystem.Entities;

namespace ManagementSystem.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token);
}
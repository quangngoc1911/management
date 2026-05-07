using System.Security.Claims;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token);
}


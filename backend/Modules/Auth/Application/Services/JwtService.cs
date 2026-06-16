using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using ManagementSystem.Application.Options;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Application.Contracts;

namespace ManagementSystem.Modules.Auth.Application.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;
    private readonly IDateTime _dateTime;
    public JwtService(IDateTime dateTime,IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
        _dateTime = dateTime;
    }


    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.SecretKey));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
        };

        // Gắn role vào token để [Authorize(Roles = ...)] hoạt động.
        // Yêu cầu user được nạp kèm UserRoles.Role (xem UserRepository.GetByEmailAsync).
        foreach (var roleName in user.UserRoles
                     .Where(ur => ur.Role != null && !string.IsNullOrWhiteSpace(ur.Role!.Name))
                     .Select(ur => ur.Role!.Name)
                     .Distinct())
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
        }

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: _dateTime.UtcNow.AddMinutes(_settings.AccessTokenExpiryMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.SecretKey));

        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _settings.Issuer,
                    ValidAudience = _settings.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                }, out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}



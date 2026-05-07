using Microsoft.Extensions.DependencyInjection;
using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Auth.Application.Services;

namespace ManagementSystem.Modules.Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        return services;
    }
}

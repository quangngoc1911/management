using Microsoft.Extensions.DependencyInjection;
using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Users.Application.Services;
using ManagementSystem.Modules.Users.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}

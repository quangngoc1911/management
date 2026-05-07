using Microsoft.Extensions.DependencyInjection;
using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Menus.Application.Services;
using ManagementSystem.Modules.Menus.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Menus.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddMenusModule(this IServiceCollection services)
    {
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        return services;
    }
}

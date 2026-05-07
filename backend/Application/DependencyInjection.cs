using Microsoft.Extensions.DependencyInjection;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Auth.Application;
using ManagementSystem.Modules.Categories.Application;
using ManagementSystem.Modules.Documents.Application;
using ManagementSystem.Modules.Menus.Application;
using ManagementSystem.Modules.Users.Application;
using ManagementSystem.Application.Services;
using ManagementSystem.Infrastructure.Repositories;
using ManagementSystem.Infrastructure.Services;

namespace ManagementSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAuthModule();
        services.AddCategoriesModule();
        services.AddDocumentsModule();
        services.AddMenusModule();
        services.AddUsersModule();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IDateTime, DateTimeService>();
        services.AddScoped<IFileStorageService, FileStorageService>();

        return services;
    }
}

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.Services;

using ManagementSystem.Modules.Auth.Application;
using ManagementSystem.Modules.Categories.Application;
using ManagementSystem.Modules.Documents.Application;
using ManagementSystem.Modules.Users.Application;

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
        services.AddUsersModule();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFileStorageService, FileStorageService>();

        services.AddSingleton<IDateTime, DateTimeService>();

        return services;
    }
}

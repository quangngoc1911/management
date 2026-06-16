using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.Services;

using ManagementSystem.Modules.Auth.Application;
using ManagementSystem.Modules.Categories.Application;
using ManagementSystem.Modules.Documents.Application;
using ManagementSystem.Modules.Users.Application;
using ManagementSystem.Modules.Family.Application;
using ManagementSystem.Modules.Finance.Application;
using ManagementSystem.Modules.Medical.Application;
using ManagementSystem.Modules.Education.Application;
using ManagementSystem.Modules.Events.Application;
using ManagementSystem.Modules.Assets.Application;
using ManagementSystem.Modules.Utility.Application;
using ManagementSystem.Modules.SystemAdmin.Application;

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
        services.AddFamilyModule();
        services.AddFinanceModule();
        services.AddMedicalModule();
        services.AddEducationModule();
        services.AddEventsModule();
        services.AddAssetsModule();
        services.AddUtilityModule();
        services.AddSystemModule();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFileStorageService, FileStorageService>();

        services.AddSingleton<IDateTime, DateTimeService>();

        return services;
    }
}

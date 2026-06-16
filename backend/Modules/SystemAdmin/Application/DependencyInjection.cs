using Microsoft.Extensions.DependencyInjection;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.SystemAdmin.Application.Services;
using ManagementSystem.Modules.SystemAdmin.Infrastructure.Repositories;

namespace ManagementSystem.Modules.SystemAdmin.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddSystemModule(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ISystemConfigService, SystemConfigService>();
        services.AddScoped<ISystemConfigRepository, SystemConfigRepository>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IBackupLogService, BackupLogService>();
        services.AddScoped<IBackupLogRepository, BackupLogRepository>();
        return services;
    }
}

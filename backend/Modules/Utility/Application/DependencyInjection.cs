using Microsoft.Extensions.DependencyInjection;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Utility.Application.Services;
using ManagementSystem.Modules.Utility.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Utility.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUtilityModule(this IServiceCollection services)
    {
        services.AddScoped<IReminderService, ReminderService>();
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<IBookmarkService, BookmarkService>();
        services.AddScoped<IBookmarkRepository, BookmarkRepository>();
        services.AddScoped<IViewHistoryService, ViewHistoryService>();
        services.AddScoped<IViewHistoryRepository, ViewHistoryRepository>();
        return services;
    }
}

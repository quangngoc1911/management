using Microsoft.Extensions.DependencyInjection;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Events.Application.Services;
using ManagementSystem.Modules.Events.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Events.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddEventsModule(this IServiceCollection services)
    {
        services.AddScoped<IFamilyEventService, FamilyEventService>();
        services.AddScoped<IFamilyEventRepository, FamilyEventRepository>();
        services.AddScoped<IEventMediaService, EventMediaService>();
        services.AddScoped<IEventMediaRepository, EventMediaRepository>();
        return services;
    }
}

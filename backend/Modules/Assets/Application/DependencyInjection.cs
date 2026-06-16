using Microsoft.Extensions.DependencyInjection;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Assets.Application.Services;
using ManagementSystem.Modules.Assets.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Assets.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAssetsModule(this IServiceCollection services)
    {
        services.AddScoped<IAssetService, AssetService>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IAssetValuationService, AssetValuationService>();
        services.AddScoped<IAssetValuationRepository, AssetValuationRepository>();
        return services;
    }
}

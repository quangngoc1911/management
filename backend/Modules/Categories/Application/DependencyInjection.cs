using Microsoft.Extensions.DependencyInjection;
using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Categories.Application.Services;
using ManagementSystem.Modules.Categories.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Categories.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCategoriesModule(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
}

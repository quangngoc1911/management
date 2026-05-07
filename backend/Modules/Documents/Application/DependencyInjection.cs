using Microsoft.Extensions.DependencyInjection;
using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Documents.Application.Services;
using ManagementSystem.Modules.Documents.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Documents.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddDocumentsModule(this IServiceCollection services)
    {
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        return services;
    }
}

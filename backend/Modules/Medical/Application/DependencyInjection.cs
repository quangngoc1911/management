using Microsoft.Extensions.DependencyInjection;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Medical.Application.Services;
using ManagementSystem.Modules.Medical.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Medical.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddMedicalModule(this IServiceCollection services)
    {
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
        services.AddScoped<IMedicationService, MedicationService>();
        services.AddScoped<IMedicationRepository, MedicationRepository>();
        services.AddScoped<IHealthMetricService, HealthMetricService>();
        services.AddScoped<IHealthMetricRepository, HealthMetricRepository>();
        return services;
    }
}

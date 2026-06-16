using Microsoft.Extensions.DependencyInjection;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Education.Application.Services;
using ManagementSystem.Modules.Education.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Education.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddEducationModule(this IServiceCollection services)
    {
        services.AddScoped<IEducationRecordService, EducationRecordService>();
        services.AddScoped<IEducationRecordRepository, EducationRecordRepository>();
        services.AddScoped<IStudyScheduleService, StudyScheduleService>();
        services.AddScoped<IStudyScheduleRepository, StudyScheduleRepository>();
        return services;
    }
}

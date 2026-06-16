using ManagementSystem.Modules.Medical.Application.DTOs;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IHealthMetricRepository
{
    Task<(IReadOnlyList<HealthMetric> Items, int TotalCount)> GetPagedAsync(HealthMetricQueryParams query);
    Task<HealthMetric?> GetByIdAsync(Guid id);
    Task<HealthMetric?> GetForUpdateAsync(Guid id);
    Task CreateAsync(HealthMetric metric);
    Task<bool> DeleteAsync(Guid id);
}

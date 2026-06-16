using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Medical.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IHealthMetricService
{
    Task<PaginatedResultDto<HealthMetricDto>> GetPagedAsync(HealthMetricQueryParams query);
    Task<HealthMetricDto?> GetByIdAsync(Guid id);
    Task<HealthMetricDto?> CreateAsync(CreateHealthMetricDto dto);
    Task<HealthMetricDto?> UpdateAsync(Guid id, UpdateHealthMetricDto dto);
    Task<bool> DeleteAsync(Guid id);
}

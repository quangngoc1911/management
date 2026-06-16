using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface ISystemConfigService
{
    Task<PaginatedResultDto<SystemConfigDto>> GetPagedAsync(SystemConfigQueryParams query);
    Task<SystemConfigDto?> GetByIdAsync(Guid id);
    Task<SystemConfigDto> CreateAsync(CreateSystemConfigDto dto);
    Task<SystemConfigDto?> UpdateAsync(Guid id, UpdateSystemConfigDto dto);
    Task<bool> DeleteAsync(Guid id);
}

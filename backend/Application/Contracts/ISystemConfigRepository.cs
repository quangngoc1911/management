using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface ISystemConfigRepository
{
    Task<(IReadOnlyList<SystemConfig> Items, int TotalCount)> GetPagedAsync(SystemConfigQueryParams query);
    Task<SystemConfig?> GetByIdAsync(Guid id);
    Task<SystemConfig?> GetForUpdateAsync(Guid id);
    Task<bool> KeyExistsAsync(string key, Guid? excludeId = null);
    Task CreateAsync(SystemConfig config);
    Task<bool> DeleteAsync(Guid id);
}

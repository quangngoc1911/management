using ManagementSystem.Modules.Events.Application.DTOs;
using ManagementSystem.Modules.Events.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IFamilyEventRepository
{
    Task<(IReadOnlyList<FamilyEvent> Items, int TotalCount)> GetPagedAsync(FamilyEventQueryParams query);
    Task<FamilyEvent?> GetByIdAsync(Guid id);
    Task<FamilyEvent?> GetForUpdateAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task CreateAsync(FamilyEvent ev);
    Task<bool> DeleteAsync(Guid id);
}

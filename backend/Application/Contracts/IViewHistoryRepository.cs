using ManagementSystem.Modules.Utility.Application.DTOs;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IViewHistoryRepository
{
    Task<(IReadOnlyList<ViewHistory> Items, int TotalCount)> GetPagedAsync(ViewHistoryQueryParams query);
    Task<ViewHistory?> GetByIdAsync(Guid id);
    Task CreateAsync(ViewHistory history);
    Task<bool> DeleteAsync(Guid id);
}

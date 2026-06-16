using ManagementSystem.Modules.Events.Application.DTOs;
using ManagementSystem.Modules.Events.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IEventMediaRepository
{
    Task<(IReadOnlyList<EventMedia> Items, int TotalCount)> GetPagedAsync(EventMediaQueryParams query);
    Task<EventMedia?> GetByIdAsync(Guid id);
    Task<EventMedia?> GetForUpdateAsync(Guid id);
    Task CreateAsync(EventMedia media);
    Task<bool> DeleteAsync(Guid id);
}

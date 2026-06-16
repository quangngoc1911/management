using ManagementSystem.Modules.Utility.Application.DTOs;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IReminderRepository
{
    Task<(IReadOnlyList<Reminder> Items, int TotalCount)> GetPagedAsync(ReminderQueryParams query);
    Task<Reminder?> GetByIdAsync(Guid id);
    Task<Reminder?> GetForUpdateAsync(Guid id);
    Task CreateAsync(Reminder reminder);
    Task<bool> DeleteAsync(Guid id);
}

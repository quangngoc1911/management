using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface INotificationRepository
{
    Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetPagedAsync(NotificationQueryParams query);
    Task<Notification?> GetByIdAsync(Guid id);
    Task<Notification?> GetForUpdateAsync(Guid id);
    Task CreateAsync(Notification notification);
    Task<bool> DeleteAsync(Guid id);
}

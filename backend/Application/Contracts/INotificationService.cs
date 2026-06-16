using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface INotificationService
{
    Task<PaginatedResultDto<NotificationDto>> GetPagedAsync(NotificationQueryParams query);
    Task<NotificationDto?> GetByIdAsync(Guid id);
    Task<NotificationDto> CreateAsync(CreateNotificationDto dto);
    Task<NotificationDto?> UpdateAsync(Guid id, UpdateNotificationDto dto);
    Task<bool> DeleteAsync(Guid id);
}

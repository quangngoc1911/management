using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Utility.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IReminderService
{
    Task<PaginatedResultDto<ReminderDto>> GetPagedAsync(ReminderQueryParams query);
    Task<ReminderDto?> GetByIdAsync(Guid id);
    Task<ReminderDto> CreateAsync(CreateReminderDto dto);
    Task<ReminderDto?> UpdateAsync(Guid id, UpdateReminderDto dto);
    Task<bool> DeleteAsync(Guid id);
}

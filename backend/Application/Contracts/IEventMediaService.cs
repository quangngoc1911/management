using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Events.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IEventMediaService
{
    Task<PaginatedResultDto<EventMediaDto>> GetPagedAsync(EventMediaQueryParams query);
    Task<EventMediaDto?> GetByIdAsync(Guid id);
    Task<EventMediaDto?> CreateAsync(CreateEventMediaDto dto);
    Task<EventMediaDto?> UpdateAsync(Guid id, UpdateEventMediaDto dto);
    Task<bool> DeleteAsync(Guid id);
}

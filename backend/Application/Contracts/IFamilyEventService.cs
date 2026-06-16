using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Events.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IFamilyEventService
{
    Task<PaginatedResultDto<FamilyEventDto>> GetPagedAsync(FamilyEventQueryParams query);
    Task<FamilyEventDto?> GetByIdAsync(Guid id);
    Task<FamilyEventDto> CreateAsync(CreateFamilyEventDto dto);
    Task<FamilyEventDto?> UpdateAsync(Guid id, UpdateFamilyEventDto dto);
    Task<bool> DeleteAsync(Guid id);
}

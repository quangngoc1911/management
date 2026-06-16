using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Utility.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IViewHistoryService
{
    Task<PaginatedResultDto<ViewHistoryDto>> GetPagedAsync(ViewHistoryQueryParams query);
    Task<ViewHistoryDto> CreateAsync(CreateViewHistoryDto dto);
    Task<bool> DeleteAsync(Guid id);
}

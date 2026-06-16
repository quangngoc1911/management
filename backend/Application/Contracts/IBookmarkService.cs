using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Utility.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IBookmarkService
{
    Task<PaginatedResultDto<BookmarkDto>> GetPagedAsync(BookmarkQueryParams query);
    Task<BookmarkDto?> GetByIdAsync(Guid id);
    Task<BookmarkDto> CreateAsync(CreateBookmarkDto dto);
    Task<BookmarkDto?> UpdateAsync(Guid id, UpdateBookmarkDto dto);
    Task<bool> DeleteAsync(Guid id);
}

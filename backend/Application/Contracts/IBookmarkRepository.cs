using ManagementSystem.Modules.Utility.Application.DTOs;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IBookmarkRepository
{
    Task<(IReadOnlyList<Bookmark> Items, int TotalCount)> GetPagedAsync(BookmarkQueryParams query);
    Task<Bookmark?> GetByIdAsync(Guid id);
    Task<Bookmark?> GetForUpdateAsync(Guid id);
    Task CreateAsync(Bookmark bookmark);
    Task<bool> DeleteAsync(Guid id);
}

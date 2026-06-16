using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Utility.Application.DTOs;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Modules.Utility.Infrastructure.Repositories;

public class BookmarkRepository : IBookmarkRepository
{
    private readonly ApplicationDbContext _context;

    public BookmarkRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Bookmark> Items, int TotalCount)> GetPagedAsync(BookmarkQueryParams query)
    {
        var q = _context.Bookmarks.AsNoTracking().AsQueryable();

        if (query.UserId.HasValue)
            q = q.Where(b => b.UserId == query.UserId);
        if (!string.IsNullOrWhiteSpace(query.EntityType))
            q = q.Where(b => b.EntityType == query.EntityType);

        q = query.IsDescending ? q.OrderByDescending(b => b.CreatedAt) : q.OrderBy(b => b.CreatedAt);

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<Bookmark?> GetByIdAsync(Guid id) =>
        await _context.Bookmarks.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

    public async Task<Bookmark?> GetForUpdateAsync(Guid id) =>
        await _context.Bookmarks.FirstOrDefaultAsync(b => b.Id == id);

    public async Task CreateAsync(Bookmark bookmark) => await _context.Bookmarks.AddAsync(bookmark);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var bookmark = await _context.Bookmarks.FirstOrDefaultAsync(b => b.Id == id);
        if (bookmark == null) return false;
        bookmark.IsDeleted = true;
        return true;
    }
}

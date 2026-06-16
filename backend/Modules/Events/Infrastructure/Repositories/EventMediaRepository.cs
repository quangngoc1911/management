using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Events.Application.DTOs;
using ManagementSystem.Modules.Events.Domain.Entities;

namespace ManagementSystem.Modules.Events.Infrastructure.Repositories;

public class EventMediaRepository : IEventMediaRepository
{
    private readonly ApplicationDbContext _context;

    public EventMediaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<EventMedia> Items, int TotalCount)> GetPagedAsync(EventMediaQueryParams query)
    {
        var q = _context.EventMedia.AsNoTracking().AsQueryable();

        if (query.EventId.HasValue)
            q = q.Where(m => m.EventId == query.EventId);

        q = query.SortBy?.ToLower() switch
        {
            "createdat" => query.IsDescending ? q.OrderByDescending(m => m.CreatedAt) : q.OrderBy(m => m.CreatedAt),
            _ => query.IsDescending ? q.OrderByDescending(m => m.SortOrder) : q.OrderBy(m => m.SortOrder)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<EventMedia?> GetByIdAsync(Guid id) =>
        await _context.EventMedia.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

    public async Task<EventMedia?> GetForUpdateAsync(Guid id) =>
        await _context.EventMedia.FirstOrDefaultAsync(m => m.Id == id);

    public async Task CreateAsync(EventMedia media) => await _context.EventMedia.AddAsync(media);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var media = await _context.EventMedia.FirstOrDefaultAsync(m => m.Id == id);
        if (media == null) return false;
        media.IsDeleted = true;
        return true;
    }
}

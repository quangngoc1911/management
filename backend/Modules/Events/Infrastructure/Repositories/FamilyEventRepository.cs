using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Events.Application.DTOs;
using ManagementSystem.Modules.Events.Domain.Entities;

namespace ManagementSystem.Modules.Events.Infrastructure.Repositories;

public class FamilyEventRepository : IFamilyEventRepository
{
    private readonly ApplicationDbContext _context;

    public FamilyEventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<FamilyEvent> Items, int TotalCount)> GetPagedAsync(FamilyEventQueryParams query)
    {
        var q = _context.FamilyEvents.AsNoTracking().Include(e => e.CreatedByUser).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            q = q.Where(e => e.Title.ToLower().Contains(search) || (e.Description != null && e.Description.ToLower().Contains(search)));
        }
        if (!string.IsNullOrWhiteSpace(query.EventType))
            q = q.Where(e => e.EventType == query.EventType);
        if (query.Status.HasValue)
            q = q.Where(e => e.Status == query.Status);
        if (query.FromDate.HasValue)
            q = q.Where(e => e.StartAt >= query.FromDate.Value);
        if (query.ToDate.HasValue)
            q = q.Where(e => e.StartAt <= query.ToDate.Value);

        q = query.SortBy?.ToLower() switch
        {
            "title" => query.IsDescending ? q.OrderByDescending(e => e.Title) : q.OrderBy(e => e.Title),
            _ => query.IsDescending ? q.OrderByDescending(e => e.StartAt) : q.OrderBy(e => e.StartAt)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<FamilyEvent?> GetByIdAsync(Guid id) =>
        await _context.FamilyEvents.AsNoTracking().Include(e => e.CreatedByUser).FirstOrDefaultAsync(e => e.Id == id);

    public async Task<FamilyEvent?> GetForUpdateAsync(Guid id) =>
        await _context.FamilyEvents.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<bool> ExistsAsync(Guid id) =>
        await _context.FamilyEvents.AnyAsync(e => e.Id == id);

    public async Task CreateAsync(FamilyEvent ev) => await _context.FamilyEvents.AddAsync(ev);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var ev = await _context.FamilyEvents.FirstOrDefaultAsync(e => e.Id == id);
        if (ev == null) return false;
        ev.IsDeleted = true;
        return true;
    }
}

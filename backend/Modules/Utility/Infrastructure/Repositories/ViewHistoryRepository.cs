using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Utility.Application.DTOs;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Modules.Utility.Infrastructure.Repositories;

public class ViewHistoryRepository : IViewHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public ViewHistoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<ViewHistory> Items, int TotalCount)> GetPagedAsync(ViewHistoryQueryParams query)
    {
        var q = _context.ViewHistories.AsNoTracking().AsQueryable();

        if (query.UserId.HasValue)
            q = q.Where(v => v.UserId == query.UserId);
        if (!string.IsNullOrWhiteSpace(query.EntityType))
            q = q.Where(v => v.EntityType == query.EntityType);

        q = q.OrderByDescending(v => v.ViewedAt);

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<ViewHistory?> GetByIdAsync(Guid id) =>
        await _context.ViewHistories.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

    public async Task CreateAsync(ViewHistory history) => await _context.ViewHistories.AddAsync(history);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var history = await _context.ViewHistories.FirstOrDefaultAsync(v => v.Id == id);
        if (history == null) return false;
        history.IsDeleted = true;
        return true;
    }
}

using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Utility.Application.DTOs;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Modules.Utility.Infrastructure.Repositories;

public class ReminderRepository : IReminderRepository
{
    private readonly ApplicationDbContext _context;

    public ReminderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Reminder> Items, int TotalCount)> GetPagedAsync(ReminderQueryParams query)
    {
        var q = _context.Reminders.AsNoTracking().Include(r => r.Member).AsQueryable();

        if (query.UserId.HasValue)
            q = q.Where(r => r.UserId == query.UserId);
        if (query.Status.HasValue)
            q = q.Where(r => r.Status == query.Status);
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            q = q.Where(r => r.Title.ToLower().Contains(search));
        }

        q = query.SortBy?.ToLower() switch
        {
            "title" => query.IsDescending ? q.OrderByDescending(r => r.Title) : q.OrderBy(r => r.Title),
            _ => query.IsDescending ? q.OrderByDescending(r => r.RemindAt) : q.OrderBy(r => r.RemindAt)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<Reminder?> GetByIdAsync(Guid id) =>
        await _context.Reminders.AsNoTracking().Include(r => r.Member).FirstOrDefaultAsync(r => r.Id == id);

    public async Task<Reminder?> GetForUpdateAsync(Guid id) =>
        await _context.Reminders.FirstOrDefaultAsync(r => r.Id == id);

    public async Task CreateAsync(Reminder reminder) => await _context.Reminders.AddAsync(reminder);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var reminder = await _context.Reminders.FirstOrDefaultAsync(r => r.Id == id);
        if (reminder == null) return false;
        reminder.IsDeleted = true;
        return true;
    }
}

using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Notification> Items, int TotalCount)> GetPagedAsync(NotificationQueryParams query)
    {
        var q = _context.Notifications.AsNoTracking().AsQueryable();

        if (query.UserId.HasValue)
            q = q.Where(n => n.UserId == query.UserId);
        if (query.IsRead.HasValue)
            q = q.Where(n => n.IsRead == query.IsRead);
        if (!string.IsNullOrWhiteSpace(query.Type))
            q = q.Where(n => n.Type == query.Type);

        q = q.OrderByDescending(n => n.CreatedAt);

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<Notification?> GetByIdAsync(Guid id) =>
        await _context.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);

    public async Task<Notification?> GetForUpdateAsync(Guid id) =>
        await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);

    public async Task CreateAsync(Notification notification) => await _context.Notifications.AddAsync(notification);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        if (notification == null) return false;
        notification.IsDeleted = true;
        return true;
    }
}

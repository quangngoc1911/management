using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly ApplicationDbContext _context;

    public AuditLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<AuditLog> Items, int TotalCount)> GetPagedAsync(AuditLogQueryParams query)
    {
        var q = _context.AuditLogs.AsNoTracking().Include(a => a.User).AsQueryable();

        if (query.UserId.HasValue)
            q = q.Where(a => a.UserId == query.UserId);
        if (query.Action.HasValue)
            q = q.Where(a => a.Action == query.Action);
        if (!string.IsNullOrWhiteSpace(query.EntityType))
            q = q.Where(a => a.EntityType == query.EntityType);
        if (query.FromDate.HasValue)
            q = q.Where(a => a.CreatedAt >= query.FromDate.Value);
        if (query.ToDate.HasValue)
            q = q.Where(a => a.CreatedAt <= query.ToDate.Value);

        q = q.OrderByDescending(a => a.CreatedAt);

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<AuditLog?> GetByIdAsync(Guid id) =>
        await _context.AuditLogs.AsNoTracking().Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
}

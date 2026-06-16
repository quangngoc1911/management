using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Infrastructure.Repositories;

public class BackupLogRepository : IBackupLogRepository
{
    private readonly ApplicationDbContext _context;

    public BackupLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<BackupLog> Items, int TotalCount)> GetPagedAsync(BackupLogQueryParams query)
    {
        var q = _context.BackupLogs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.BackupType))
            q = q.Where(b => b.BackupType == query.BackupType);
        if (!string.IsNullOrWhiteSpace(query.Status))
            q = q.Where(b => b.Status == query.Status);

        // BackupLog does not map audit fields (CreatedAt is unmapped); order by the mapped StartedAt.
        q = q.OrderByDescending(b => b.StartedAt);

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<BackupLog?> GetByIdAsync(Guid id) =>
        await _context.BackupLogs.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
}

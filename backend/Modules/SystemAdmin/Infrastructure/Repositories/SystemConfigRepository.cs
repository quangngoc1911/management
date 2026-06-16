using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Infrastructure.Repositories;

public class SystemConfigRepository : ISystemConfigRepository
{
    private readonly ApplicationDbContext _context;

    public SystemConfigRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<SystemConfig> Items, int TotalCount)> GetPagedAsync(SystemConfigQueryParams query)
    {
        var q = _context.SystemConfigs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            q = q.Where(c => c.Key.ToLower().Contains(search) || (c.Description != null && c.Description.ToLower().Contains(search)));
        }
        if (query.IsPublic.HasValue)
            q = q.Where(c => c.IsPublic == query.IsPublic);

        q = query.IsDescending ? q.OrderByDescending(c => c.Key) : q.OrderBy(c => c.Key);

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<SystemConfig?> GetByIdAsync(Guid id) =>
        await _context.SystemConfigs.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

    public async Task<SystemConfig?> GetForUpdateAsync(Guid id) =>
        await _context.SystemConfigs.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> KeyExistsAsync(string key, Guid? excludeId = null)
    {
        var q = _context.SystemConfigs.Where(c => c.Key == key);
        if (excludeId.HasValue)
            q = q.Where(c => c.Id != excludeId.Value);
        return await q.AnyAsync();
    }

    public async Task CreateAsync(SystemConfig config) => await _context.SystemConfigs.AddAsync(config);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var config = await _context.SystemConfigs.FirstOrDefaultAsync(c => c.Id == id);
        if (config == null) return false;
        config.IsDeleted = true;
        return true;
    }
}

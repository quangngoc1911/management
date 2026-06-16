using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Medical.Application.DTOs;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Modules.Medical.Infrastructure.Repositories;

public class HealthMetricRepository : IHealthMetricRepository
{
    private readonly ApplicationDbContext _context;

    public HealthMetricRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<HealthMetric> Items, int TotalCount)> GetPagedAsync(HealthMetricQueryParams query)
    {
        var q = _context.HealthMetrics.AsNoTracking().Include(h => h.Member).AsQueryable();

        if (query.MemberId.HasValue)
            q = q.Where(h => h.MemberId == query.MemberId);
        if (!string.IsNullOrWhiteSpace(query.MetricType))
            q = q.Where(h => h.MetricType == query.MetricType);
        if (query.FromDate.HasValue)
            q = q.Where(h => h.MeasuredAt >= query.FromDate.Value);
        if (query.ToDate.HasValue)
            q = q.Where(h => h.MeasuredAt <= query.ToDate.Value);

        q = query.SortBy?.ToLower() switch
        {
            "metrictype" => query.IsDescending ? q.OrderByDescending(h => h.MetricType) : q.OrderBy(h => h.MetricType),
            _ => query.IsDescending ? q.OrderByDescending(h => h.MeasuredAt) : q.OrderBy(h => h.MeasuredAt)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<HealthMetric?> GetByIdAsync(Guid id) =>
        await _context.HealthMetrics.AsNoTracking().Include(h => h.Member).FirstOrDefaultAsync(h => h.Id == id);

    public async Task<HealthMetric?> GetForUpdateAsync(Guid id) =>
        await _context.HealthMetrics.FirstOrDefaultAsync(h => h.Id == id);

    public async Task CreateAsync(HealthMetric metric) => await _context.HealthMetrics.AddAsync(metric);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var metric = await _context.HealthMetrics.FirstOrDefaultAsync(h => h.Id == id);
        if (metric == null) return false;
        metric.IsDeleted = true;
        return true;
    }
}

using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Education.Application.DTOs;
using ManagementSystem.Modules.Education.Domain.Entities;

namespace ManagementSystem.Modules.Education.Infrastructure.Repositories;

public class EducationRecordRepository : IEducationRecordRepository
{
    private readonly ApplicationDbContext _context;

    public EducationRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<EducationRecord> Items, int TotalCount)> GetPagedAsync(EducationRecordQueryParams query)
    {
        var q = _context.EducationRecords.AsNoTracking().Include(r => r.Member).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            q = q.Where(r => r.InstitutionName.ToLower().Contains(search) || (r.Major != null && r.Major.ToLower().Contains(search)));
        }
        if (query.MemberId.HasValue)
            q = q.Where(r => r.MemberId == query.MemberId);
        if (!string.IsNullOrWhiteSpace(query.Level))
            q = q.Where(r => r.Level == query.Level);
        if (query.Status.HasValue)
            q = q.Where(r => r.Status == query.Status);

        q = query.SortBy?.ToLower() switch
        {
            "institutionname" => query.IsDescending ? q.OrderByDescending(r => r.InstitutionName) : q.OrderBy(r => r.InstitutionName),
            _ => query.IsDescending ? q.OrderByDescending(r => r.StartDate) : q.OrderBy(r => r.StartDate)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<EducationRecord?> GetByIdAsync(Guid id) =>
        await _context.EducationRecords.AsNoTracking().Include(r => r.Member).FirstOrDefaultAsync(r => r.Id == id);

    public async Task<EducationRecord?> GetForUpdateAsync(Guid id) =>
        await _context.EducationRecords.FirstOrDefaultAsync(r => r.Id == id);

    public async Task CreateAsync(EducationRecord record) => await _context.EducationRecords.AddAsync(record);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var record = await _context.EducationRecords.FirstOrDefaultAsync(r => r.Id == id);
        if (record == null) return false;
        record.IsDeleted = true;
        return true;
    }
}

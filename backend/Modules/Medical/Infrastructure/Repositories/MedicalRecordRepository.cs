using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Medical.Application.DTOs;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Modules.Medical.Infrastructure.Repositories;

public class MedicalRecordRepository : IMedicalRecordRepository
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<MedicalRecord> Items, int TotalCount)> GetPagedAsync(MedicalRecordQueryParams query)
    {
        var q = _context.MedicalRecords.AsNoTracking().Include(r => r.Member).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            q = q.Where(r => r.Title.ToLower().Contains(search) || (r.Diagnosis != null && r.Diagnosis.ToLower().Contains(search)));
        }
        if (query.MemberId.HasValue)
            q = q.Where(r => r.MemberId == query.MemberId);
        if (!string.IsNullOrWhiteSpace(query.RecordType))
            q = q.Where(r => r.RecordType == query.RecordType);
        if (query.FromDate.HasValue)
            q = q.Where(r => r.RecordDate >= query.FromDate.Value);
        if (query.ToDate.HasValue)
            q = q.Where(r => r.RecordDate <= query.ToDate.Value);

        q = query.SortBy?.ToLower() switch
        {
            "title" => query.IsDescending ? q.OrderByDescending(r => r.Title) : q.OrderBy(r => r.Title),
            _ => query.IsDescending ? q.OrderByDescending(r => r.RecordDate) : q.OrderBy(r => r.RecordDate)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<MedicalRecord?> GetByIdAsync(Guid id) =>
        await _context.MedicalRecords.AsNoTracking().Include(r => r.Member).FirstOrDefaultAsync(r => r.Id == id);

    public async Task<MedicalRecord?> GetForUpdateAsync(Guid id) =>
        await _context.MedicalRecords.FirstOrDefaultAsync(r => r.Id == id);

    public async Task CreateAsync(MedicalRecord record) => await _context.MedicalRecords.AddAsync(record);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var record = await _context.MedicalRecords.FirstOrDefaultAsync(r => r.Id == id);
        if (record == null) return false;
        record.IsDeleted = true;
        return true;
    }
}

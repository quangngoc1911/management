using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Medical.Application.DTOs;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Modules.Medical.Infrastructure.Repositories;

public class MedicationRepository : IMedicationRepository
{
    private readonly ApplicationDbContext _context;

    public MedicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Medication> Items, int TotalCount)> GetPagedAsync(MedicationQueryParams query)
    {
        var q = _context.Medications.AsNoTracking().Include(m => m.Member).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            q = q.Where(m => m.Name.ToLower().Contains(search));
        }
        if (query.MemberId.HasValue)
            q = q.Where(m => m.MemberId == query.MemberId);
        if (query.IsActive.HasValue)
            q = q.Where(m => m.IsActive == query.IsActive);

        q = query.SortBy?.ToLower() switch
        {
            "startdate" => query.IsDescending ? q.OrderByDescending(m => m.StartDate) : q.OrderBy(m => m.StartDate),
            _ => query.IsDescending ? q.OrderByDescending(m => m.Name) : q.OrderBy(m => m.Name)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<Medication?> GetByIdAsync(Guid id) =>
        await _context.Medications.AsNoTracking().Include(m => m.Member).FirstOrDefaultAsync(m => m.Id == id);

    public async Task<Medication?> GetForUpdateAsync(Guid id) =>
        await _context.Medications.FirstOrDefaultAsync(m => m.Id == id);

    public async Task CreateAsync(Medication medication) => await _context.Medications.AddAsync(medication);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var medication = await _context.Medications.FirstOrDefaultAsync(m => m.Id == id);
        if (medication == null) return false;
        medication.IsDeleted = true;
        return true;
    }
}

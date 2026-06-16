using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Assets.Application.DTOs;
using ManagementSystem.Modules.Assets.Domain.Entities;

namespace ManagementSystem.Modules.Assets.Infrastructure.Repositories;

public class AssetValuationRepository : IAssetValuationRepository
{
    private readonly ApplicationDbContext _context;

    public AssetValuationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<AssetValuation> Items, int TotalCount)> GetPagedAsync(AssetValuationQueryParams query)
    {
        var q = _context.AssetValuations.AsNoTracking().Include(v => v.Asset).AsQueryable();

        if (query.AssetId.HasValue)
            q = q.Where(v => v.AssetId == query.AssetId);

        q = query.SortBy?.ToLower() switch
        {
            "value" => query.IsDescending ? q.OrderByDescending(v => v.Value) : q.OrderBy(v => v.Value),
            _ => query.IsDescending ? q.OrderByDescending(v => v.ValuationDate) : q.OrderBy(v => v.ValuationDate)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<AssetValuation?> GetByIdAsync(Guid id) =>
        await _context.AssetValuations.AsNoTracking().Include(v => v.Asset).FirstOrDefaultAsync(v => v.Id == id);

    public async Task<AssetValuation?> GetForUpdateAsync(Guid id) =>
        await _context.AssetValuations.FirstOrDefaultAsync(v => v.Id == id);

    public async Task CreateAsync(AssetValuation valuation) => await _context.AssetValuations.AddAsync(valuation);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var valuation = await _context.AssetValuations.FirstOrDefaultAsync(v => v.Id == id);
        if (valuation == null) return false;
        valuation.IsDeleted = true;
        return true;
    }
}

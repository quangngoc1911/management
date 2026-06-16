using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Assets.Application.DTOs;
using ManagementSystem.Modules.Assets.Domain.Entities;

namespace ManagementSystem.Modules.Assets.Infrastructure.Repositories;

public class AssetRepository : IAssetRepository
{
    private readonly ApplicationDbContext _context;

    public AssetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Asset> Items, int TotalCount)> GetPagedAsync(AssetQueryParams query)
    {
        var q = _context.Assets
            .AsNoTracking()
            .Include(a => a.Member)
            .Include(a => a.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            q = q.Where(a => a.Name.ToLower().Contains(search) || (a.Description != null && a.Description.ToLower().Contains(search)));
        }
        if (query.MemberId.HasValue)
            q = q.Where(a => a.MemberId == query.MemberId);
        if (query.CategoryId.HasValue)
            q = q.Where(a => a.CategoryId == query.CategoryId);
        if (!string.IsNullOrWhiteSpace(query.AssetType))
            q = q.Where(a => a.AssetType == query.AssetType);
        if (query.Status.HasValue)
            q = q.Where(a => a.Status == query.Status);

        q = query.SortBy?.ToLower() switch
        {
            "purchasedate" => query.IsDescending ? q.OrderByDescending(a => a.PurchaseDate) : q.OrderBy(a => a.PurchaseDate),
            _ => query.IsDescending ? q.OrderByDescending(a => a.Name) : q.OrderBy(a => a.Name)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<Asset?> GetByIdAsync(Guid id) =>
        await _context.Assets.AsNoTracking().Include(a => a.Member).Include(a => a.Category).FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Asset?> GetForUpdateAsync(Guid id) =>
        await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);

    public async Task<bool> ExistsAsync(Guid id) =>
        await _context.Assets.AnyAsync(a => a.Id == id);

    public async Task CreateAsync(Asset asset) => await _context.Assets.AddAsync(asset);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);
        if (asset == null) return false;
        asset.IsDeleted = true;
        return true;
    }
}

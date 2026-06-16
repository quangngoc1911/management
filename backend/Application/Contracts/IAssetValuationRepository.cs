using ManagementSystem.Modules.Assets.Application.DTOs;
using ManagementSystem.Modules.Assets.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IAssetValuationRepository
{
    Task<(IReadOnlyList<AssetValuation> Items, int TotalCount)> GetPagedAsync(AssetValuationQueryParams query);
    Task<AssetValuation?> GetByIdAsync(Guid id);
    Task<AssetValuation?> GetForUpdateAsync(Guid id);
    Task CreateAsync(AssetValuation valuation);
    Task<bool> DeleteAsync(Guid id);
}

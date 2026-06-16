using ManagementSystem.Modules.Assets.Application.DTOs;
using ManagementSystem.Modules.Assets.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IAssetRepository
{
    Task<(IReadOnlyList<Asset> Items, int TotalCount)> GetPagedAsync(AssetQueryParams query);
    Task<Asset?> GetByIdAsync(Guid id);
    Task<Asset?> GetForUpdateAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task CreateAsync(Asset asset);
    Task<bool> DeleteAsync(Guid id);
}

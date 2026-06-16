using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Assets.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IAssetService
{
    Task<PaginatedResultDto<AssetDto>> GetPagedAsync(AssetQueryParams query);
    Task<AssetDto?> GetByIdAsync(Guid id);
    Task<AssetDto> CreateAsync(CreateAssetDto dto);
    Task<AssetDto?> UpdateAsync(Guid id, UpdateAssetDto dto);
    Task<bool> DeleteAsync(Guid id);
}

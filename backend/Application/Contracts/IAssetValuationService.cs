using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Assets.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IAssetValuationService
{
    Task<PaginatedResultDto<AssetValuationDto>> GetPagedAsync(AssetValuationQueryParams query);
    Task<AssetValuationDto?> GetByIdAsync(Guid id);
    Task<AssetValuationDto?> CreateAsync(CreateAssetValuationDto dto);
    Task<AssetValuationDto?> UpdateAsync(Guid id, UpdateAssetValuationDto dto);
    Task<bool> DeleteAsync(Guid id);
}

using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Assets.Application.DTOs;
using ManagementSystem.Modules.Assets.Domain.Entities;

namespace ManagementSystem.Modules.Assets.Application.Services;

public class AssetService : IAssetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public AssetService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<AssetDto>> GetPagedAsync(AssetQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.Assets.GetPagedAsync(query);
        return new PaginatedResultDto<AssetDto>
        {
            Items = _mapper.Map<List<AssetDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<AssetDto?> GetByIdAsync(Guid id)
    {
        var asset = await _unitOfWork.Assets.GetByIdAsync(id);
        return asset is null ? null : _mapper.Map<AssetDto>(asset);
    }

    public async Task<AssetDto> CreateAsync(CreateAssetDto dto)
    {
        var asset = _mapper.Map<Asset>(dto);
        asset.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.Assets.CreateAsync(asset);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.Assets.GetByIdAsync(asset.Id);
        return _mapper.Map<AssetDto>(created);
    }

    public async Task<AssetDto?> UpdateAsync(Guid id, UpdateAssetDto dto)
    {
        var asset = await _unitOfWork.Assets.GetForUpdateAsync(id);
        if (asset is null) return null;

        _mapper.Map(dto, asset);
        asset.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.Assets.GetByIdAsync(id);
        return _mapper.Map<AssetDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Assets.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

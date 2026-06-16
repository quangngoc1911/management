using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Assets.Application.DTOs;
using ManagementSystem.Modules.Assets.Domain.Entities;

namespace ManagementSystem.Modules.Assets.Application.Services;

public class AssetValuationService : IAssetValuationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public AssetValuationService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<AssetValuationDto>> GetPagedAsync(AssetValuationQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.AssetValuations.GetPagedAsync(query);
        return new PaginatedResultDto<AssetValuationDto>
        {
            Items = _mapper.Map<List<AssetValuationDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<AssetValuationDto?> GetByIdAsync(Guid id)
    {
        var valuation = await _unitOfWork.AssetValuations.GetByIdAsync(id);
        return valuation is null ? null : _mapper.Map<AssetValuationDto>(valuation);
    }

    public async Task<AssetValuationDto?> CreateAsync(CreateAssetValuationDto dto)
    {
        if (!await _unitOfWork.Assets.ExistsAsync(dto.AssetId))
        {
            return null;
        }

        var valuation = _mapper.Map<AssetValuation>(dto);
        valuation.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.AssetValuations.CreateAsync(valuation);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.AssetValuations.GetByIdAsync(valuation.Id);
        return _mapper.Map<AssetValuationDto>(created);
    }

    public async Task<AssetValuationDto?> UpdateAsync(Guid id, UpdateAssetValuationDto dto)
    {
        var valuation = await _unitOfWork.AssetValuations.GetForUpdateAsync(id);
        if (valuation is null) return null;

        _mapper.Map(dto, valuation);
        valuation.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.AssetValuations.GetByIdAsync(id);
        return _mapper.Map<AssetValuationDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.AssetValuations.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

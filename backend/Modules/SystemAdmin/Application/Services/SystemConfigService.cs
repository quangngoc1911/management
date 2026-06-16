using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Application.Services;

public class SystemConfigService : ISystemConfigService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public SystemConfigService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<SystemConfigDto>> GetPagedAsync(SystemConfigQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.SystemConfigs.GetPagedAsync(query);
        return new PaginatedResultDto<SystemConfigDto>
        {
            Items = _mapper.Map<List<SystemConfigDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<SystemConfigDto?> GetByIdAsync(Guid id)
    {
        var config = await _unitOfWork.SystemConfigs.GetByIdAsync(id);
        return config is null ? null : _mapper.Map<SystemConfigDto>(config);
    }

    public async Task<SystemConfigDto> CreateAsync(CreateSystemConfigDto dto)
    {
        if (await _unitOfWork.SystemConfigs.KeyExistsAsync(dto.Key))
        {
            throw new InvalidOperationException("Khoá cấu hình đã tồn tại");
        }

        var config = _mapper.Map<SystemConfig>(dto);
        config.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.SystemConfigs.CreateAsync(config);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.SystemConfigs.GetByIdAsync(config.Id);
        return _mapper.Map<SystemConfigDto>(created);
    }

    public async Task<SystemConfigDto?> UpdateAsync(Guid id, UpdateSystemConfigDto dto)
    {
        var config = await _unitOfWork.SystemConfigs.GetForUpdateAsync(id);
        if (config is null) return null;

        _mapper.Map(dto, config);
        config.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.SystemConfigs.GetByIdAsync(id);
        return _mapper.Map<SystemConfigDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.SystemConfigs.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

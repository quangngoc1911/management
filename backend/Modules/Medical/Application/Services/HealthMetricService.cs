using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Medical.Application.DTOs;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Modules.Medical.Application.Services;

public class HealthMetricService : IHealthMetricService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public HealthMetricService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<HealthMetricDto>> GetPagedAsync(HealthMetricQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.HealthMetrics.GetPagedAsync(query);
        return new PaginatedResultDto<HealthMetricDto>
        {
            Items = _mapper.Map<List<HealthMetricDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<HealthMetricDto?> GetByIdAsync(Guid id)
    {
        var metric = await _unitOfWork.HealthMetrics.GetByIdAsync(id);
        return metric is null ? null : _mapper.Map<HealthMetricDto>(metric);
    }

    public async Task<HealthMetricDto?> CreateAsync(CreateHealthMetricDto dto)
    {
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId))
        {
            return null;
        }

        var metric = _mapper.Map<HealthMetric>(dto);
        metric.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.HealthMetrics.CreateAsync(metric);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.HealthMetrics.GetByIdAsync(metric.Id);
        return _mapper.Map<HealthMetricDto>(created);
    }

    public async Task<HealthMetricDto?> UpdateAsync(Guid id, UpdateHealthMetricDto dto)
    {
        var metric = await _unitOfWork.HealthMetrics.GetForUpdateAsync(id);
        if (metric is null) return null;
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId)) return null;

        _mapper.Map(dto, metric);
        metric.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.HealthMetrics.GetByIdAsync(id);
        return _mapper.Map<HealthMetricDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.HealthMetrics.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

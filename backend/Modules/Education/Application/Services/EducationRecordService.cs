using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Education.Application.DTOs;
using ManagementSystem.Modules.Education.Domain.Entities;

namespace ManagementSystem.Modules.Education.Application.Services;

public class EducationRecordService : IEducationRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public EducationRecordService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<EducationRecordDto>> GetPagedAsync(EducationRecordQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.EducationRecords.GetPagedAsync(query);
        return new PaginatedResultDto<EducationRecordDto>
        {
            Items = _mapper.Map<List<EducationRecordDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<EducationRecordDto?> GetByIdAsync(Guid id)
    {
        var record = await _unitOfWork.EducationRecords.GetByIdAsync(id);
        return record is null ? null : _mapper.Map<EducationRecordDto>(record);
    }

    public async Task<EducationRecordDto?> CreateAsync(CreateEducationRecordDto dto)
    {
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId))
        {
            return null;
        }

        var record = _mapper.Map<EducationRecord>(dto);
        record.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.EducationRecords.CreateAsync(record);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.EducationRecords.GetByIdAsync(record.Id);
        return _mapper.Map<EducationRecordDto>(created);
    }

    public async Task<EducationRecordDto?> UpdateAsync(Guid id, UpdateEducationRecordDto dto)
    {
        var record = await _unitOfWork.EducationRecords.GetForUpdateAsync(id);
        if (record is null) return null;
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId)) return null;

        _mapper.Map(dto, record);
        record.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.EducationRecords.GetByIdAsync(id);
        return _mapper.Map<EducationRecordDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.EducationRecords.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

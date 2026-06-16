using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Medical.Application.DTOs;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Modules.Medical.Application.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public MedicalRecordService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<MedicalRecordDto>> GetPagedAsync(MedicalRecordQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.MedicalRecords.GetPagedAsync(query);
        return new PaginatedResultDto<MedicalRecordDto>
        {
            Items = _mapper.Map<List<MedicalRecordDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<MedicalRecordDto?> GetByIdAsync(Guid id)
    {
        var record = await _unitOfWork.MedicalRecords.GetByIdAsync(id);
        return record is null ? null : _mapper.Map<MedicalRecordDto>(record);
    }

    public async Task<MedicalRecordDto?> CreateAsync(CreateMedicalRecordDto dto)
    {
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId))
        {
            return null;
        }

        var record = _mapper.Map<MedicalRecord>(dto);
        record.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.MedicalRecords.CreateAsync(record);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.MedicalRecords.GetByIdAsync(record.Id);
        return _mapper.Map<MedicalRecordDto>(created);
    }

    public async Task<MedicalRecordDto?> UpdateAsync(Guid id, UpdateMedicalRecordDto dto)
    {
        var record = await _unitOfWork.MedicalRecords.GetForUpdateAsync(id);
        if (record is null) return null;
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId)) return null;

        _mapper.Map(dto, record);
        record.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.MedicalRecords.GetByIdAsync(id);
        return _mapper.Map<MedicalRecordDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.MedicalRecords.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

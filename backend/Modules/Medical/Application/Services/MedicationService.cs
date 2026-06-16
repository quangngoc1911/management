using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Medical.Application.DTOs;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Modules.Medical.Application.Services;

public class MedicationService : IMedicationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public MedicationService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<MedicationDto>> GetPagedAsync(MedicationQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.Medications.GetPagedAsync(query);
        return new PaginatedResultDto<MedicationDto>
        {
            Items = _mapper.Map<List<MedicationDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<MedicationDto?> GetByIdAsync(Guid id)
    {
        var medication = await _unitOfWork.Medications.GetByIdAsync(id);
        return medication is null ? null : _mapper.Map<MedicationDto>(medication);
    }

    public async Task<MedicationDto?> CreateAsync(CreateMedicationDto dto)
    {
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId))
        {
            return null;
        }

        var medication = _mapper.Map<Medication>(dto);
        medication.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.Medications.CreateAsync(medication);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.Medications.GetByIdAsync(medication.Id);
        return _mapper.Map<MedicationDto>(created);
    }

    public async Task<MedicationDto?> UpdateAsync(Guid id, UpdateMedicationDto dto)
    {
        var medication = await _unitOfWork.Medications.GetForUpdateAsync(id);
        if (medication is null) return null;
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId)) return null;

        _mapper.Map(dto, medication);
        medication.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.Medications.GetByIdAsync(id);
        return _mapper.Map<MedicationDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Medications.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

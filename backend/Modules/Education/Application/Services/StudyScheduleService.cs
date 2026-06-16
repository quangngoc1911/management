using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Education.Application.DTOs;
using ManagementSystem.Modules.Education.Domain.Entities;

namespace ManagementSystem.Modules.Education.Application.Services;

public class StudyScheduleService : IStudyScheduleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public StudyScheduleService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<StudyScheduleDto>> GetPagedAsync(StudyScheduleQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.StudySchedules.GetPagedAsync(query);
        return new PaginatedResultDto<StudyScheduleDto>
        {
            Items = _mapper.Map<List<StudyScheduleDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<StudyScheduleDto?> GetByIdAsync(Guid id)
    {
        var schedule = await _unitOfWork.StudySchedules.GetByIdAsync(id);
        return schedule is null ? null : _mapper.Map<StudyScheduleDto>(schedule);
    }

    public async Task<StudyScheduleDto?> CreateAsync(CreateStudyScheduleDto dto)
    {
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId))
        {
            return null;
        }

        var schedule = _mapper.Map<StudySchedule>(dto);
        schedule.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.StudySchedules.CreateAsync(schedule);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.StudySchedules.GetByIdAsync(schedule.Id);
        return _mapper.Map<StudyScheduleDto>(created);
    }

    public async Task<StudyScheduleDto?> UpdateAsync(Guid id, UpdateStudyScheduleDto dto)
    {
        var schedule = await _unitOfWork.StudySchedules.GetForUpdateAsync(id);
        if (schedule is null) return null;
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId)) return null;

        _mapper.Map(dto, schedule);
        schedule.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.StudySchedules.GetByIdAsync(id);
        return _mapper.Map<StudyScheduleDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.StudySchedules.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

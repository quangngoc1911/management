using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Utility.Application.DTOs;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Modules.Utility.Application.Services;

public class ReminderService : IReminderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public ReminderService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<ReminderDto>> GetPagedAsync(ReminderQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.Reminders.GetPagedAsync(query);
        return new PaginatedResultDto<ReminderDto>
        {
            Items = _mapper.Map<List<ReminderDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<ReminderDto?> GetByIdAsync(Guid id)
    {
        var reminder = await _unitOfWork.Reminders.GetByIdAsync(id);
        return reminder is null ? null : _mapper.Map<ReminderDto>(reminder);
    }

    public async Task<ReminderDto> CreateAsync(CreateReminderDto dto)
    {
        var reminder = _mapper.Map<Reminder>(dto);
        reminder.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.Reminders.CreateAsync(reminder);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.Reminders.GetByIdAsync(reminder.Id);
        return _mapper.Map<ReminderDto>(created);
    }

    public async Task<ReminderDto?> UpdateAsync(Guid id, UpdateReminderDto dto)
    {
        var reminder = await _unitOfWork.Reminders.GetForUpdateAsync(id);
        if (reminder is null) return null;

        _mapper.Map(dto, reminder);
        reminder.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.Reminders.GetByIdAsync(id);
        return _mapper.Map<ReminderDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Reminders.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

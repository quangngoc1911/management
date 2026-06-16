using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<NotificationDto>> GetPagedAsync(NotificationQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.Notifications.GetPagedAsync(query);
        return new PaginatedResultDto<NotificationDto>
        {
            Items = _mapper.Map<List<NotificationDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<NotificationDto?> GetByIdAsync(Guid id)
    {
        var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
        return notification is null ? null : _mapper.Map<NotificationDto>(notification);
    }

    public async Task<NotificationDto> CreateAsync(CreateNotificationDto dto)
    {
        var notification = _mapper.Map<Notification>(dto);
        notification.SentAt = _dateTime.UtcNow;
        notification.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.Notifications.CreateAsync(notification);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.Notifications.GetByIdAsync(notification.Id);
        return _mapper.Map<NotificationDto>(created);
    }

    public async Task<NotificationDto?> UpdateAsync(Guid id, UpdateNotificationDto dto)
    {
        var notification = await _unitOfWork.Notifications.GetForUpdateAsync(id);
        if (notification is null) return null;

        notification.IsRead = dto.IsRead;
        notification.ReadAt = dto.IsRead ? (notification.ReadAt ?? _dateTime.UtcNow) : null;
        notification.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.Notifications.GetByIdAsync(id);
        return _mapper.Map<NotificationDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Notifications.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

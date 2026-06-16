using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Events.Application.DTOs;
using ManagementSystem.Modules.Events.Domain.Entities;

namespace ManagementSystem.Modules.Events.Application.Services;

public class EventMediaService : IEventMediaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public EventMediaService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<EventMediaDto>> GetPagedAsync(EventMediaQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.EventMedia.GetPagedAsync(query);
        return new PaginatedResultDto<EventMediaDto>
        {
            Items = _mapper.Map<List<EventMediaDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<EventMediaDto?> GetByIdAsync(Guid id)
    {
        var media = await _unitOfWork.EventMedia.GetByIdAsync(id);
        return media is null ? null : _mapper.Map<EventMediaDto>(media);
    }

    public async Task<EventMediaDto?> CreateAsync(CreateEventMediaDto dto)
    {
        if (!await _unitOfWork.FamilyEvents.ExistsAsync(dto.EventId))
        {
            return null;
        }

        var media = _mapper.Map<EventMedia>(dto);
        media.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.EventMedia.CreateAsync(media);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.EventMedia.GetByIdAsync(media.Id);
        return _mapper.Map<EventMediaDto>(created);
    }

    public async Task<EventMediaDto?> UpdateAsync(Guid id, UpdateEventMediaDto dto)
    {
        var media = await _unitOfWork.EventMedia.GetForUpdateAsync(id);
        if (media is null) return null;

        _mapper.Map(dto, media);
        media.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.EventMedia.GetByIdAsync(id);
        return _mapper.Map<EventMediaDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.EventMedia.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

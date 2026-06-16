using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Events.Application.DTOs;
using ManagementSystem.Modules.Events.Domain.Entities;

namespace ManagementSystem.Modules.Events.Application.Services;

public class FamilyEventService : IFamilyEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public FamilyEventService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<FamilyEventDto>> GetPagedAsync(FamilyEventQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.FamilyEvents.GetPagedAsync(query);
        return new PaginatedResultDto<FamilyEventDto>
        {
            Items = _mapper.Map<List<FamilyEventDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<FamilyEventDto?> GetByIdAsync(Guid id)
    {
        var ev = await _unitOfWork.FamilyEvents.GetByIdAsync(id);
        return ev is null ? null : _mapper.Map<FamilyEventDto>(ev);
    }

    public async Task<FamilyEventDto> CreateAsync(CreateFamilyEventDto dto)
    {
        var ev = _mapper.Map<FamilyEvent>(dto);
        ev.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.FamilyEvents.CreateAsync(ev);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.FamilyEvents.GetByIdAsync(ev.Id);
        return _mapper.Map<FamilyEventDto>(created);
    }

    public async Task<FamilyEventDto?> UpdateAsync(Guid id, UpdateFamilyEventDto dto)
    {
        var ev = await _unitOfWork.FamilyEvents.GetForUpdateAsync(id);
        if (ev is null) return null;

        _mapper.Map(dto, ev);
        ev.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.FamilyEvents.GetByIdAsync(id);
        return _mapper.Map<FamilyEventDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.FamilyEvents.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

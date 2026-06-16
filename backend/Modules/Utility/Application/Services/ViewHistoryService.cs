using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Utility.Application.DTOs;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Modules.Utility.Application.Services;

public class ViewHistoryService : IViewHistoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public ViewHistoryService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<ViewHistoryDto>> GetPagedAsync(ViewHistoryQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.ViewHistories.GetPagedAsync(query);
        return new PaginatedResultDto<ViewHistoryDto>
        {
            Items = _mapper.Map<List<ViewHistoryDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<ViewHistoryDto> CreateAsync(CreateViewHistoryDto dto)
    {
        var history = _mapper.Map<ViewHistory>(dto);
        if (history.ViewedAt == default)
        {
            history.ViewedAt = _dateTime.UtcNow;
        }
        history.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.ViewHistories.CreateAsync(history);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.ViewHistories.GetByIdAsync(history.Id);
        return _mapper.Map<ViewHistoryDto>(created);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.ViewHistories.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

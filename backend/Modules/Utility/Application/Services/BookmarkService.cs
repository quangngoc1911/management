using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Utility.Application.DTOs;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Modules.Utility.Application.Services;

public class BookmarkService : IBookmarkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public BookmarkService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<BookmarkDto>> GetPagedAsync(BookmarkQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.Bookmarks.GetPagedAsync(query);
        return new PaginatedResultDto<BookmarkDto>
        {
            Items = _mapper.Map<List<BookmarkDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<BookmarkDto?> GetByIdAsync(Guid id)
    {
        var bookmark = await _unitOfWork.Bookmarks.GetByIdAsync(id);
        return bookmark is null ? null : _mapper.Map<BookmarkDto>(bookmark);
    }

    public async Task<BookmarkDto> CreateAsync(CreateBookmarkDto dto)
    {
        var bookmark = _mapper.Map<Bookmark>(dto);
        bookmark.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.Bookmarks.CreateAsync(bookmark);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.Bookmarks.GetByIdAsync(bookmark.Id);
        return _mapper.Map<BookmarkDto>(created);
    }

    public async Task<BookmarkDto?> UpdateAsync(Guid id, UpdateBookmarkDto dto)
    {
        var bookmark = await _unitOfWork.Bookmarks.GetForUpdateAsync(id);
        if (bookmark is null) return null;

        _mapper.Map(dto, bookmark);
        bookmark.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.Bookmarks.GetByIdAsync(id);
        return _mapper.Map<BookmarkDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Bookmarks.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

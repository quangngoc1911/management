using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Utility.Application.DTOs;

namespace ManagementSystem.Modules.Utility.Presentation.Controllers;

[ApiController]
[Route("api/bookmarks")]
[Authorize]
public class BookmarksController : ControllerBase
{
    private readonly IBookmarkService _service;
    private readonly ILogger<BookmarksController> _logger;

    public BookmarksController(IBookmarkService service, ILogger<BookmarksController> logger)
    {
        _service = service;
        _logger = logger;
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<BookmarkDto>>>> GetAllAsync([FromQuery] BookmarkQueryParams query)
    {
        try
        {
            query.UserId = GetCurrentUserId();
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<BookmarkDto>>.SuccessResult(result, "Lấy danh sách đánh dấu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookmarks");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<BookmarkDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách đánh dấu", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<BookmarkDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var bookmark = await _service.GetByIdAsync(id);
            if (bookmark is null)
                return NotFound(ApiResponseDto<BookmarkDto>.ErrorResult("Không tìm thấy đánh dấu", 404));
            return Ok(ApiResponseDto<BookmarkDto>.SuccessResult(bookmark, "Lấy thông tin đánh dấu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookmark {Id}", id);
            return StatusCode(500, ApiResponseDto<BookmarkDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin đánh dấu", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponseDto<BookmarkDto>>> CreateAsync([FromBody] CreateBookmarkDto dto)
    {
        try
        {
            dto.UserId = GetCurrentUserId();
            var bookmark = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = bookmark.Id },
                ApiResponseDto<BookmarkDto>.SuccessResult(bookmark, "Tạo đánh dấu thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bookmark");
            return StatusCode(500, ApiResponseDto<BookmarkDto>.ErrorResult("Có lỗi xảy ra khi tạo đánh dấu", 500));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<BookmarkDto>>> UpdateAsync(Guid id, [FromBody] UpdateBookmarkDto dto)
    {
        try
        {
            var bookmark = await _service.UpdateAsync(id, dto);
            if (bookmark is null)
                return NotFound(ApiResponseDto<BookmarkDto>.ErrorResult("Không tìm thấy đánh dấu", 404));
            return Ok(ApiResponseDto<BookmarkDto>.SuccessResult(bookmark, "Cập nhật đánh dấu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bookmark {Id}", id);
            return StatusCode(500, ApiResponseDto<BookmarkDto>.ErrorResult("Có lỗi xảy ra khi cập nhật đánh dấu", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy đánh dấu", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa đánh dấu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bookmark {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa đánh dấu", 500));
        }
    }
}

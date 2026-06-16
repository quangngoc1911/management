using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Events.Application.DTOs;

namespace ManagementSystem.Modules.Events.Presentation.Controllers;

[ApiController]
[Route("api/event-media")]
[Authorize]
public class EventMediaController : ControllerBase
{
    private readonly IEventMediaService _service;
    private readonly ILogger<EventMediaController> _logger;

    public EventMediaController(IEventMediaService service, ILogger<EventMediaController> logger)
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
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<EventMediaDto>>>> GetAllAsync([FromQuery] EventMediaQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<EventMediaDto>>.SuccessResult(result, "Lấy danh sách media sự kiện thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting event media");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<EventMediaDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách media", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<EventMediaDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var media = await _service.GetByIdAsync(id);
            if (media is null)
                return NotFound(ApiResponseDto<EventMediaDto>.ErrorResult("Không tìm thấy media", 404));
            return Ok(ApiResponseDto<EventMediaDto>.SuccessResult(media, "Lấy thông tin media thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting event media {Id}", id);
            return StatusCode(500, ApiResponseDto<EventMediaDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin media", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<EventMediaDto>>> CreateAsync([FromBody] CreateEventMediaDto dto)
    {
        try
        {
            dto.UploadedByUserId = GetCurrentUserId();
            var media = await _service.CreateAsync(dto);
            if (media is null)
                return NotFound(ApiResponseDto<EventMediaDto>.ErrorResult("Không tìm thấy sự kiện", 404));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = media.Id },
                ApiResponseDto<EventMediaDto>.SuccessResult(media, "Thêm media thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event media");
            return StatusCode(500, ApiResponseDto<EventMediaDto>.ErrorResult("Có lỗi xảy ra khi thêm media", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<EventMediaDto>>> UpdateAsync(Guid id, [FromBody] UpdateEventMediaDto dto)
    {
        try
        {
            var media = await _service.UpdateAsync(id, dto);
            if (media is null)
                return NotFound(ApiResponseDto<EventMediaDto>.ErrorResult("Không tìm thấy media", 404));
            return Ok(ApiResponseDto<EventMediaDto>.SuccessResult(media, "Cập nhật media thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event media {Id}", id);
            return StatusCode(500, ApiResponseDto<EventMediaDto>.ErrorResult("Có lỗi xảy ra khi cập nhật media", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy media", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa media thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event media {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa media", 500));
        }
    }
}

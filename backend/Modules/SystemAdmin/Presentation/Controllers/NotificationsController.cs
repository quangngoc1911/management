using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Presentation.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(INotificationService service, ILogger<NotificationsController> logger)
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
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<NotificationDto>>>> GetAllAsync([FromQuery] NotificationQueryParams query)
    {
        try
        {
            query.UserId = GetCurrentUserId();
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<NotificationDto>>.SuccessResult(result, "Lấy danh sách thông báo thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notifications");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<NotificationDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách thông báo", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<NotificationDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var notification = await _service.GetByIdAsync(id);
            if (notification is null)
                return NotFound(ApiResponseDto<NotificationDto>.ErrorResult("Không tìm thấy thông báo", 404));
            return Ok(ApiResponseDto<NotificationDto>.SuccessResult(notification, "Lấy thông tin thông báo thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notification {Id}", id);
            return StatusCode(500, ApiResponseDto<NotificationDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin thông báo", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<NotificationDto>>> CreateAsync([FromBody] CreateNotificationDto dto)
    {
        try
        {
            var notification = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = notification.Id },
                ApiResponseDto<NotificationDto>.SuccessResult(notification, "Tạo thông báo thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification");
            return StatusCode(500, ApiResponseDto<NotificationDto>.ErrorResult("Có lỗi xảy ra khi tạo thông báo", 500));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<NotificationDto>>> UpdateAsync(Guid id, [FromBody] UpdateNotificationDto dto)
    {
        try
        {
            var notification = await _service.UpdateAsync(id, dto);
            if (notification is null)
                return NotFound(ApiResponseDto<NotificationDto>.ErrorResult("Không tìm thấy thông báo", 404));
            return Ok(ApiResponseDto<NotificationDto>.SuccessResult(notification, "Cập nhật thông báo thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating notification {Id}", id);
            return StatusCode(500, ApiResponseDto<NotificationDto>.ErrorResult("Có lỗi xảy ra khi cập nhật thông báo", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy thông báo", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa thông báo thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting notification {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa thông báo", 500));
        }
    }
}

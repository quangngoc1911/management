using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Utility.Application.DTOs;

namespace ManagementSystem.Modules.Utility.Presentation.Controllers;

[ApiController]
[Route("api/reminders")]
[Authorize]
public class RemindersController : ControllerBase
{
    private readonly IReminderService _service;
    private readonly ILogger<RemindersController> _logger;

    public RemindersController(IReminderService service, ILogger<RemindersController> logger)
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
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<ReminderDto>>>> GetAllAsync([FromQuery] ReminderQueryParams query)
    {
        try
        {
            query.UserId = GetCurrentUserId(); // chỉ trả nhắc nhở của chính người dùng
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<ReminderDto>>.SuccessResult(result, "Lấy danh sách nhắc nhở thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reminders");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<ReminderDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách nhắc nhở", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<ReminderDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var reminder = await _service.GetByIdAsync(id);
            if (reminder is null)
                return NotFound(ApiResponseDto<ReminderDto>.ErrorResult("Không tìm thấy nhắc nhở", 404));
            return Ok(ApiResponseDto<ReminderDto>.SuccessResult(reminder, "Lấy thông tin nhắc nhở thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reminder {Id}", id);
            return StatusCode(500, ApiResponseDto<ReminderDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin nhắc nhở", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponseDto<ReminderDto>>> CreateAsync([FromBody] CreateReminderDto dto)
    {
        try
        {
            dto.UserId = GetCurrentUserId();
            var reminder = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = reminder.Id },
                ApiResponseDto<ReminderDto>.SuccessResult(reminder, "Tạo nhắc nhở thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reminder");
            return StatusCode(500, ApiResponseDto<ReminderDto>.ErrorResult("Có lỗi xảy ra khi tạo nhắc nhở", 500));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<ReminderDto>>> UpdateAsync(Guid id, [FromBody] UpdateReminderDto dto)
    {
        try
        {
            var reminder = await _service.UpdateAsync(id, dto);
            if (reminder is null)
                return NotFound(ApiResponseDto<ReminderDto>.ErrorResult("Không tìm thấy nhắc nhở", 404));
            return Ok(ApiResponseDto<ReminderDto>.SuccessResult(reminder, "Cập nhật nhắc nhở thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating reminder {Id}", id);
            return StatusCode(500, ApiResponseDto<ReminderDto>.ErrorResult("Có lỗi xảy ra khi cập nhật nhắc nhở", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy nhắc nhở", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa nhắc nhở thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting reminder {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa nhắc nhở", 500));
        }
    }
}

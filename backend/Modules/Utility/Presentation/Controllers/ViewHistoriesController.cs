using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Utility.Application.DTOs;

namespace ManagementSystem.Modules.Utility.Presentation.Controllers;

[ApiController]
[Route("api/view-histories")]
[Authorize]
public class ViewHistoriesController : ControllerBase
{
    private readonly IViewHistoryService _service;
    private readonly ILogger<ViewHistoriesController> _logger;

    public ViewHistoriesController(IViewHistoryService service, ILogger<ViewHistoriesController> logger)
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
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<ViewHistoryDto>>>> GetAllAsync([FromQuery] ViewHistoryQueryParams query)
    {
        try
        {
            query.UserId = GetCurrentUserId();
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<ViewHistoryDto>>.SuccessResult(result, "Lấy lịch sử xem thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting view histories");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<ViewHistoryDto>>.ErrorResult("Có lỗi xảy ra khi lấy lịch sử xem", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponseDto<ViewHistoryDto>>> CreateAsync([FromBody] CreateViewHistoryDto dto)
    {
        try
        {
            dto.UserId = GetCurrentUserId();
            var history = await _service.CreateAsync(dto);
            return Ok(ApiResponseDto<ViewHistoryDto>.SuccessResult(history, "Ghi nhận lượt xem thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating view history");
            return StatusCode(500, ApiResponseDto<ViewHistoryDto>.ErrorResult("Có lỗi xảy ra khi ghi nhận lượt xem", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy lịch sử xem", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa lịch sử xem thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting view history {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa lịch sử xem", 500));
        }
    }
}

using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Events.Application.DTOs;

namespace ManagementSystem.Modules.Events.Presentation.Controllers;

[ApiController]
[Route("api/family-events")]
[Authorize]
public class FamilyEventsController : ControllerBase
{
    private readonly IFamilyEventService _service;
    private readonly ILogger<FamilyEventsController> _logger;

    public FamilyEventsController(IFamilyEventService service, ILogger<FamilyEventsController> logger)
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
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<FamilyEventDto>>>> GetAllAsync([FromQuery] FamilyEventQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<FamilyEventDto>>.SuccessResult(result, "Lấy danh sách sự kiện thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting family events");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<FamilyEventDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách sự kiện", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<FamilyEventDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var ev = await _service.GetByIdAsync(id);
            if (ev is null)
                return NotFound(ApiResponseDto<FamilyEventDto>.ErrorResult("Không tìm thấy sự kiện", 404));
            return Ok(ApiResponseDto<FamilyEventDto>.SuccessResult(ev, "Lấy thông tin sự kiện thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting family event {Id}", id);
            return StatusCode(500, ApiResponseDto<FamilyEventDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin sự kiện", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<FamilyEventDto>>> CreateAsync([FromBody] CreateFamilyEventDto dto)
    {
        try
        {
            dto.CreatedByUserId = GetCurrentUserId();
            var ev = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = ev.Id },
                ApiResponseDto<FamilyEventDto>.SuccessResult(ev, "Tạo sự kiện thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating family event");
            return StatusCode(500, ApiResponseDto<FamilyEventDto>.ErrorResult("Có lỗi xảy ra khi tạo sự kiện", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<FamilyEventDto>>> UpdateAsync(Guid id, [FromBody] UpdateFamilyEventDto dto)
    {
        try
        {
            var ev = await _service.UpdateAsync(id, dto);
            if (ev is null)
                return NotFound(ApiResponseDto<FamilyEventDto>.ErrorResult("Không tìm thấy sự kiện", 404));
            return Ok(ApiResponseDto<FamilyEventDto>.SuccessResult(ev, "Cập nhật sự kiện thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating family event {Id}", id);
            return StatusCode(500, ApiResponseDto<FamilyEventDto>.ErrorResult("Có lỗi xảy ra khi cập nhật sự kiện", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy sự kiện", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa sự kiện thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting family event {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa sự kiện", 500));
        }
    }
}

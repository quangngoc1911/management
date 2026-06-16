using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Assets.Application.DTOs;

namespace ManagementSystem.Modules.Assets.Presentation.Controllers;

[ApiController]
[Route("api/asset-valuations")]
[Authorize]
public class AssetValuationsController : ControllerBase
{
    private readonly IAssetValuationService _service;
    private readonly ILogger<AssetValuationsController> _logger;

    public AssetValuationsController(IAssetValuationService service, ILogger<AssetValuationsController> logger)
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
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<AssetValuationDto>>>> GetAllAsync([FromQuery] AssetValuationQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<AssetValuationDto>>.SuccessResult(result, "Lấy danh sách định giá thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting asset valuations");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<AssetValuationDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách định giá", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<AssetValuationDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var valuation = await _service.GetByIdAsync(id);
            if (valuation is null)
                return NotFound(ApiResponseDto<AssetValuationDto>.ErrorResult("Không tìm thấy bản định giá", 404));
            return Ok(ApiResponseDto<AssetValuationDto>.SuccessResult(valuation, "Lấy thông tin định giá thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting asset valuation {Id}", id);
            return StatusCode(500, ApiResponseDto<AssetValuationDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin định giá", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<AssetValuationDto>>> CreateAsync([FromBody] CreateAssetValuationDto dto)
    {
        try
        {
            dto.CreatedByUserId = GetCurrentUserId();
            var valuation = await _service.CreateAsync(dto);
            if (valuation is null)
                return NotFound(ApiResponseDto<AssetValuationDto>.ErrorResult("Không tìm thấy tài sản", 404));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = valuation.Id },
                ApiResponseDto<AssetValuationDto>.SuccessResult(valuation, "Tạo bản định giá thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating asset valuation");
            return StatusCode(500, ApiResponseDto<AssetValuationDto>.ErrorResult("Có lỗi xảy ra khi tạo bản định giá", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<AssetValuationDto>>> UpdateAsync(Guid id, [FromBody] UpdateAssetValuationDto dto)
    {
        try
        {
            var valuation = await _service.UpdateAsync(id, dto);
            if (valuation is null)
                return NotFound(ApiResponseDto<AssetValuationDto>.ErrorResult("Không tìm thấy bản định giá", 404));
            return Ok(ApiResponseDto<AssetValuationDto>.SuccessResult(valuation, "Cập nhật định giá thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating asset valuation {Id}", id);
            return StatusCode(500, ApiResponseDto<AssetValuationDto>.ErrorResult("Có lỗi xảy ra khi cập nhật định giá", 500));
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
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy bản định giá", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa bản định giá thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting asset valuation {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa bản định giá", 500));
        }
    }
}

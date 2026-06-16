using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Assets.Application.DTOs;

namespace ManagementSystem.Modules.Assets.Presentation.Controllers;

[ApiController]
[Route("api/assets")]
[Authorize]
public class AssetsController : ControllerBase
{
    private readonly IAssetService _service;
    private readonly ILogger<AssetsController> _logger;

    public AssetsController(IAssetService service, ILogger<AssetsController> logger)
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
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<AssetDto>>>> GetAllAsync([FromQuery] AssetQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<AssetDto>>.SuccessResult(result, "Lấy danh sách tài sản thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting assets");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<AssetDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách tài sản", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<AssetDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var asset = await _service.GetByIdAsync(id);
            if (asset is null)
                return NotFound(ApiResponseDto<AssetDto>.ErrorResult("Không tìm thấy tài sản", 404));
            return Ok(ApiResponseDto<AssetDto>.SuccessResult(asset, "Lấy thông tin tài sản thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting asset {Id}", id);
            return StatusCode(500, ApiResponseDto<AssetDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin tài sản", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<AssetDto>>> CreateAsync([FromBody] CreateAssetDto dto)
    {
        try
        {
            dto.CreatedByUserId = GetCurrentUserId();
            var asset = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = asset.Id },
                ApiResponseDto<AssetDto>.SuccessResult(asset, "Tạo tài sản thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating asset");
            return StatusCode(500, ApiResponseDto<AssetDto>.ErrorResult("Có lỗi xảy ra khi tạo tài sản", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<AssetDto>>> UpdateAsync(Guid id, [FromBody] UpdateAssetDto dto)
    {
        try
        {
            var asset = await _service.UpdateAsync(id, dto);
            if (asset is null)
                return NotFound(ApiResponseDto<AssetDto>.ErrorResult("Không tìm thấy tài sản", 404));
            return Ok(ApiResponseDto<AssetDto>.SuccessResult(asset, "Cập nhật tài sản thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating asset {Id}", id);
            return StatusCode(500, ApiResponseDto<AssetDto>.ErrorResult("Có lỗi xảy ra khi cập nhật tài sản", 500));
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
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy tài sản", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa tài sản thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting asset {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa tài sản", 500));
        }
    }
}

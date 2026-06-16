using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Presentation.Controllers;

[ApiController]
[Route("api/system-configs")]
[Authorize(Roles = "Admin")]
public class SystemConfigsController : ControllerBase
{
    private readonly ISystemConfigService _service;
    private readonly ILogger<SystemConfigsController> _logger;

    public SystemConfigsController(ISystemConfigService service, ILogger<SystemConfigsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<SystemConfigDto>>>> GetAllAsync([FromQuery] SystemConfigQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<SystemConfigDto>>.SuccessResult(result, "Lấy danh sách cấu hình thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting system configs");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<SystemConfigDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách cấu hình", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<SystemConfigDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var config = await _service.GetByIdAsync(id);
            if (config is null)
                return NotFound(ApiResponseDto<SystemConfigDto>.ErrorResult("Không tìm thấy cấu hình", 404));
            return Ok(ApiResponseDto<SystemConfigDto>.SuccessResult(config, "Lấy thông tin cấu hình thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting system config {Id}", id);
            return StatusCode(500, ApiResponseDto<SystemConfigDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin cấu hình", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponseDto<SystemConfigDto>>> CreateAsync([FromBody] CreateSystemConfigDto dto)
    {
        try
        {
            var config = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = config.Id },
                ApiResponseDto<SystemConfigDto>.SuccessResult(config, "Tạo cấu hình thành công", 201));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponseDto<SystemConfigDto>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating system config");
            return StatusCode(500, ApiResponseDto<SystemConfigDto>.ErrorResult("Có lỗi xảy ra khi tạo cấu hình", 500));
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<SystemConfigDto>>> UpdateAsync(Guid id, [FromBody] UpdateSystemConfigDto dto)
    {
        try
        {
            var config = await _service.UpdateAsync(id, dto);
            if (config is null)
                return NotFound(ApiResponseDto<SystemConfigDto>.ErrorResult("Không tìm thấy cấu hình", 404));
            return Ok(ApiResponseDto<SystemConfigDto>.SuccessResult(config, "Cập nhật cấu hình thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating system config {Id}", id);
            return StatusCode(500, ApiResponseDto<SystemConfigDto>.ErrorResult("Có lỗi xảy ra khi cập nhật cấu hình", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy cấu hình", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa cấu hình thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting system config {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa cấu hình", 500));
        }
    }
}

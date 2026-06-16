using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Medical.Application.DTOs;

namespace ManagementSystem.Modules.Medical.Presentation.Controllers;

[ApiController]
[Route("api/health-metrics")]
[Authorize]
public class HealthMetricsController : ControllerBase
{
    private readonly IHealthMetricService _service;
    private readonly ILogger<HealthMetricsController> _logger;

    public HealthMetricsController(IHealthMetricService service, ILogger<HealthMetricsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<HealthMetricDto>>>> GetAllAsync([FromQuery] HealthMetricQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<HealthMetricDto>>.SuccessResult(result, "Lấy danh sách chỉ số sức khỏe thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting health metrics");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<HealthMetricDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách chỉ số sức khỏe", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<HealthMetricDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var metric = await _service.GetByIdAsync(id);
            if (metric is null)
                return NotFound(ApiResponseDto<HealthMetricDto>.ErrorResult("Không tìm thấy chỉ số sức khỏe", 404));
            return Ok(ApiResponseDto<HealthMetricDto>.SuccessResult(metric, "Lấy thông tin chỉ số sức khỏe thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting health metric {Id}", id);
            return StatusCode(500, ApiResponseDto<HealthMetricDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin chỉ số sức khỏe", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<HealthMetricDto>>> CreateAsync([FromBody] CreateHealthMetricDto dto)
    {
        try
        {
            var metric = await _service.CreateAsync(dto);
            if (metric is null)
                return NotFound(ApiResponseDto<HealthMetricDto>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = metric.Id },
                ApiResponseDto<HealthMetricDto>.SuccessResult(metric, "Tạo chỉ số sức khỏe thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating health metric");
            return StatusCode(500, ApiResponseDto<HealthMetricDto>.ErrorResult("Có lỗi xảy ra khi tạo chỉ số sức khỏe", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<HealthMetricDto>>> UpdateAsync(Guid id, [FromBody] UpdateHealthMetricDto dto)
    {
        try
        {
            var metric = await _service.UpdateAsync(id, dto);
            if (metric is null)
                return NotFound(ApiResponseDto<HealthMetricDto>.ErrorResult("Không tìm thấy chỉ số sức khỏe hoặc thành viên", 404));
            return Ok(ApiResponseDto<HealthMetricDto>.SuccessResult(metric, "Cập nhật chỉ số sức khỏe thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating health metric {Id}", id);
            return StatusCode(500, ApiResponseDto<HealthMetricDto>.ErrorResult("Có lỗi xảy ra khi cập nhật chỉ số sức khỏe", 500));
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
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy chỉ số sức khỏe", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa chỉ số sức khỏe thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting health metric {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa chỉ số sức khỏe", 500));
        }
    }
}

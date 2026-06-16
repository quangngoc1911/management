using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Education.Application.DTOs;

namespace ManagementSystem.Modules.Education.Presentation.Controllers;

[ApiController]
[Route("api/education-records")]
[Authorize]
public class EducationRecordsController : ControllerBase
{
    private readonly IEducationRecordService _service;
    private readonly ILogger<EducationRecordsController> _logger;

    public EducationRecordsController(IEducationRecordService service, ILogger<EducationRecordsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<EducationRecordDto>>>> GetAllAsync([FromQuery] EducationRecordQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<EducationRecordDto>>.SuccessResult(result, "Lấy danh sách hồ sơ học tập thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting education records");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<EducationRecordDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách hồ sơ học tập", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<EducationRecordDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var record = await _service.GetByIdAsync(id);
            if (record is null)
                return NotFound(ApiResponseDto<EducationRecordDto>.ErrorResult("Không tìm thấy hồ sơ học tập", 404));
            return Ok(ApiResponseDto<EducationRecordDto>.SuccessResult(record, "Lấy thông tin hồ sơ học tập thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting education record {Id}", id);
            return StatusCode(500, ApiResponseDto<EducationRecordDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin hồ sơ học tập", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<EducationRecordDto>>> CreateAsync([FromBody] CreateEducationRecordDto dto)
    {
        try
        {
            var record = await _service.CreateAsync(dto);
            if (record is null)
                return NotFound(ApiResponseDto<EducationRecordDto>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = record.Id },
                ApiResponseDto<EducationRecordDto>.SuccessResult(record, "Tạo hồ sơ học tập thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating education record");
            return StatusCode(500, ApiResponseDto<EducationRecordDto>.ErrorResult("Có lỗi xảy ra khi tạo hồ sơ học tập", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<EducationRecordDto>>> UpdateAsync(Guid id, [FromBody] UpdateEducationRecordDto dto)
    {
        try
        {
            var record = await _service.UpdateAsync(id, dto);
            if (record is null)
                return NotFound(ApiResponseDto<EducationRecordDto>.ErrorResult("Không tìm thấy hồ sơ học tập hoặc thành viên", 404));
            return Ok(ApiResponseDto<EducationRecordDto>.SuccessResult(record, "Cập nhật hồ sơ học tập thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating education record {Id}", id);
            return StatusCode(500, ApiResponseDto<EducationRecordDto>.ErrorResult("Có lỗi xảy ra khi cập nhật hồ sơ học tập", 500));
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
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy hồ sơ học tập", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa hồ sơ học tập thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting education record {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa hồ sơ học tập", 500));
        }
    }
}

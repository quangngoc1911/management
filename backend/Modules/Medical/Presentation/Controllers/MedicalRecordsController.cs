using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Medical.Application.DTOs;

namespace ManagementSystem.Modules.Medical.Presentation.Controllers;

[ApiController]
[Route("api/medical-records")]
[Authorize]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMedicalRecordService _service;
    private readonly ILogger<MedicalRecordsController> _logger;

    public MedicalRecordsController(IMedicalRecordService service, ILogger<MedicalRecordsController> logger)
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
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<MedicalRecordDto>>>> GetAllAsync([FromQuery] MedicalRecordQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<MedicalRecordDto>>.SuccessResult(result, "Lấy danh sách hồ sơ y tế thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting medical records");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<MedicalRecordDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách hồ sơ y tế", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<MedicalRecordDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var record = await _service.GetByIdAsync(id);
            if (record is null)
                return NotFound(ApiResponseDto<MedicalRecordDto>.ErrorResult("Không tìm thấy hồ sơ y tế", 404));
            return Ok(ApiResponseDto<MedicalRecordDto>.SuccessResult(record, "Lấy thông tin hồ sơ y tế thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting medical record {Id}", id);
            return StatusCode(500, ApiResponseDto<MedicalRecordDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin hồ sơ y tế", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<MedicalRecordDto>>> CreateAsync([FromBody] CreateMedicalRecordDto dto)
    {
        try
        {
            dto.CreatedByUserId = GetCurrentUserId();
            var record = await _service.CreateAsync(dto);
            if (record is null)
                return NotFound(ApiResponseDto<MedicalRecordDto>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = record.Id },
                ApiResponseDto<MedicalRecordDto>.SuccessResult(record, "Tạo hồ sơ y tế thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating medical record");
            return StatusCode(500, ApiResponseDto<MedicalRecordDto>.ErrorResult("Có lỗi xảy ra khi tạo hồ sơ y tế", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<MedicalRecordDto>>> UpdateAsync(Guid id, [FromBody] UpdateMedicalRecordDto dto)
    {
        try
        {
            dto.UpdatedByUserId = GetCurrentUserId();
            var record = await _service.UpdateAsync(id, dto);
            if (record is null)
                return NotFound(ApiResponseDto<MedicalRecordDto>.ErrorResult("Không tìm thấy hồ sơ y tế hoặc thành viên", 404));
            return Ok(ApiResponseDto<MedicalRecordDto>.SuccessResult(record, "Cập nhật hồ sơ y tế thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating medical record {Id}", id);
            return StatusCode(500, ApiResponseDto<MedicalRecordDto>.ErrorResult("Có lỗi xảy ra khi cập nhật hồ sơ y tế", 500));
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
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy hồ sơ y tế", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa hồ sơ y tế thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting medical record {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa hồ sơ y tế", 500));
        }
    }
}

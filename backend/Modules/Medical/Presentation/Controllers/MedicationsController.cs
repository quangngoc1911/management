using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Medical.Application.DTOs;

namespace ManagementSystem.Modules.Medical.Presentation.Controllers;

[ApiController]
[Route("api/medications")]
[Authorize]
public class MedicationsController : ControllerBase
{
    private readonly IMedicationService _service;
    private readonly ILogger<MedicationsController> _logger;

    public MedicationsController(IMedicationService service, ILogger<MedicationsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<MedicationDto>>>> GetAllAsync([FromQuery] MedicationQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<MedicationDto>>.SuccessResult(result, "Lấy danh sách thuốc thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting medications");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<MedicationDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách thuốc", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<MedicationDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var medication = await _service.GetByIdAsync(id);
            if (medication is null)
                return NotFound(ApiResponseDto<MedicationDto>.ErrorResult("Không tìm thấy thuốc", 404));
            return Ok(ApiResponseDto<MedicationDto>.SuccessResult(medication, "Lấy thông tin thuốc thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting medication {Id}", id);
            return StatusCode(500, ApiResponseDto<MedicationDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin thuốc", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<MedicationDto>>> CreateAsync([FromBody] CreateMedicationDto dto)
    {
        try
        {
            var medication = await _service.CreateAsync(dto);
            if (medication is null)
                return NotFound(ApiResponseDto<MedicationDto>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = medication.Id },
                ApiResponseDto<MedicationDto>.SuccessResult(medication, "Tạo thuốc thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating medication");
            return StatusCode(500, ApiResponseDto<MedicationDto>.ErrorResult("Có lỗi xảy ra khi tạo thuốc", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<MedicationDto>>> UpdateAsync(Guid id, [FromBody] UpdateMedicationDto dto)
    {
        try
        {
            var medication = await _service.UpdateAsync(id, dto);
            if (medication is null)
                return NotFound(ApiResponseDto<MedicationDto>.ErrorResult("Không tìm thấy thuốc hoặc thành viên", 404));
            return Ok(ApiResponseDto<MedicationDto>.SuccessResult(medication, "Cập nhật thuốc thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating medication {Id}", id);
            return StatusCode(500, ApiResponseDto<MedicationDto>.ErrorResult("Có lỗi xảy ra khi cập nhật thuốc", 500));
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
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy thuốc", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa thuốc thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting medication {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa thuốc", 500));
        }
    }
}

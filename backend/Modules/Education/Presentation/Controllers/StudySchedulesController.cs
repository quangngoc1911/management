using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Education.Application.DTOs;

namespace ManagementSystem.Modules.Education.Presentation.Controllers;

[ApiController]
[Route("api/study-schedules")]
[Authorize]
public class StudySchedulesController : ControllerBase
{
    private readonly IStudyScheduleService _service;
    private readonly ILogger<StudySchedulesController> _logger;

    public StudySchedulesController(IStudyScheduleService service, ILogger<StudySchedulesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<StudyScheduleDto>>>> GetAllAsync([FromQuery] StudyScheduleQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<StudyScheduleDto>>.SuccessResult(result, "Lấy danh sách lịch học thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting study schedules");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<StudyScheduleDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách lịch học", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<StudyScheduleDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var schedule = await _service.GetByIdAsync(id);
            if (schedule is null)
                return NotFound(ApiResponseDto<StudyScheduleDto>.ErrorResult("Không tìm thấy lịch học", 404));
            return Ok(ApiResponseDto<StudyScheduleDto>.SuccessResult(schedule, "Lấy thông tin lịch học thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting study schedule {Id}", id);
            return StatusCode(500, ApiResponseDto<StudyScheduleDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin lịch học", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<StudyScheduleDto>>> CreateAsync([FromBody] CreateStudyScheduleDto dto)
    {
        try
        {
            var schedule = await _service.CreateAsync(dto);
            if (schedule is null)
                return NotFound(ApiResponseDto<StudyScheduleDto>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = schedule.Id },
                ApiResponseDto<StudyScheduleDto>.SuccessResult(schedule, "Tạo lịch học thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating study schedule");
            return StatusCode(500, ApiResponseDto<StudyScheduleDto>.ErrorResult("Có lỗi xảy ra khi tạo lịch học", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<StudyScheduleDto>>> UpdateAsync(Guid id, [FromBody] UpdateStudyScheduleDto dto)
    {
        try
        {
            var schedule = await _service.UpdateAsync(id, dto);
            if (schedule is null)
                return NotFound(ApiResponseDto<StudyScheduleDto>.ErrorResult("Không tìm thấy lịch học hoặc thành viên", 404));
            return Ok(ApiResponseDto<StudyScheduleDto>.SuccessResult(schedule, "Cập nhật lịch học thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating study schedule {Id}", id);
            return StatusCode(500, ApiResponseDto<StudyScheduleDto>.ErrorResult("Có lỗi xảy ra khi cập nhật lịch học", 500));
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
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy lịch học", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa lịch học thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting study schedule {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa lịch học", 500));
        }
    }
}

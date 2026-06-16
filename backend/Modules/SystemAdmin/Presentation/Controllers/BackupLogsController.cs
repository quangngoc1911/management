using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Presentation.Controllers;

[ApiController]
[Route("api/backup-logs")]
[Authorize(Roles = "Admin")]
public class BackupLogsController : ControllerBase
{
    private readonly IBackupLogService _service;
    private readonly ILogger<BackupLogsController> _logger;

    public BackupLogsController(IBackupLogService service, ILogger<BackupLogsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<BackupLogDto>>>> GetAllAsync([FromQuery] BackupLogQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<BackupLogDto>>.SuccessResult(result, "Lấy nhật ký sao lưu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting backup logs");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<BackupLogDto>>.ErrorResult("Có lỗi xảy ra khi lấy nhật ký sao lưu", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<BackupLogDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var log = await _service.GetByIdAsync(id);
            if (log is null)
                return NotFound(ApiResponseDto<BackupLogDto>.ErrorResult("Không tìm thấy bản ghi", 404));
            return Ok(ApiResponseDto<BackupLogDto>.SuccessResult(log, "Lấy thông tin sao lưu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting backup log {Id}", id);
            return StatusCode(500, ApiResponseDto<BackupLogDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin sao lưu", 500));
        }
    }
}

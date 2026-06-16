using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Presentation.Controllers;

[ApiController]
[Route("api/audit-logs")]
[Authorize(Roles = "Admin")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogService _service;
    private readonly ILogger<AuditLogsController> _logger;

    public AuditLogsController(IAuditLogService service, ILogger<AuditLogsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<AuditLogDto>>>> GetAllAsync([FromQuery] AuditLogQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<AuditLogDto>>.SuccessResult(result, "Lấy nhật ký kiểm toán thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting audit logs");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<AuditLogDto>>.ErrorResult("Có lỗi xảy ra khi lấy nhật ký kiểm toán", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<AuditLogDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var log = await _service.GetByIdAsync(id);
            if (log is null)
                return NotFound(ApiResponseDto<AuditLogDto>.ErrorResult("Không tìm thấy bản ghi", 404));
            return Ok(ApiResponseDto<AuditLogDto>.SuccessResult(log, "Lấy thông tin nhật ký thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting audit log {Id}", id);
            return StatusCode(500, ApiResponseDto<AuditLogDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin nhật ký", 500));
        }
    }
}

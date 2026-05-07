using System.Security.Claims;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Documents.Application.DTOs;
using ManagementSystem.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Modules.Documents.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentsController> _logger;

    public DocumentsController(
        IDocumentService documentService,
        ILogger<DocumentsController> logger)
    {
        _documentService = documentService;
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
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<DocumentListDto>>>> GetAllAsync(
        [FromQuery] DocumentQueryDto query)
    {
        try
        {
            var result = await _documentService.GetAllAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<DocumentListDto>>.SuccessResult(result, "Lấy danh sách tài liệu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting documents");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<DocumentListDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách tài liệu"));
        }
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponseDto<DocumentDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var document = await _documentService.GetByIdAsync(id);
            if (document == null)
            {
                return NotFound(ApiResponseDto<DocumentDto>.ErrorResult("Không tìm thấy tài liệu"));
            }
            return Ok(ApiResponseDto<DocumentDto>.SuccessResult(document, "Lấy thông tin tài liệu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting document {DocumentId}", id);
            return StatusCode(500, ApiResponseDto<DocumentDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin tài liệu"));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<DocumentDto>>> CreateAsync(
        [FromForm] CreateDocumentDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var document = await _documentService.CreateAsync(dto, userId);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = document.Id },
                ApiResponseDto<DocumentDto>.SuccessResult(document, "Tạo tài liệu thành công")
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponseDto<DocumentDto>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating document");
            return StatusCode(500, ApiResponseDto<DocumentDto>.ErrorResult("Có lỗi xảy ra khi tạo tài liệu"));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<DocumentDto>>> UpdateAsync(
        Guid id,
        [FromForm] UpdateDocumentDto dto)
    {
        try
        {
            dto.Id = id;
            var userId = GetCurrentUserId();
            var document = await _documentService.UpdateAsync(dto, userId);
            if (document == null)
            {
                return NotFound(ApiResponseDto<DocumentDto>.ErrorResult("Không tìm thấy tài liệu"));
            }
            return Ok(ApiResponseDto<DocumentDto>.SuccessResult(document, "Cập nhật tài liệu thành công"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponseDto<DocumentDto>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating document {DocumentId}", id);
            return StatusCode(500, ApiResponseDto<DocumentDto>.ErrorResult("Có lỗi xảy ra khi cập nhật tài liệu"));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _documentService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy tài liệu"));
            }
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa tài liệu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting document {DocumentId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa tài liệu"));
        }
    }

    [HttpGet("{id:guid}/download")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadFileAsync(Guid id)
    {
        try
        {
            var result = await _documentService.DownloadFileAsync(id);
            if (result == null)
            {
                return NotFound(ApiResponseDto<object>.ErrorResult("Không tìm thấy file"));
            }

            var (fileBytes, fileName, contentType) = result.Value;
            return File(fileBytes, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file for document {DocumentId}", id);
            return StatusCode(500, ApiResponseDto<object>.ErrorResult("Có lỗi xảy ra khi tải file"));
        }
    }

    [HttpGet("stats/dashboard")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<DashboardStatsDto>>> GetDashboardStatsAsync()
    {
        try
        {
            var stats = await _documentService.GetDashboardStatsAsync();
            return Ok(ApiResponseDto<DashboardStatsDto>.SuccessResult(stats, "Lấy thống kê thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard stats");
            return StatusCode(500, ApiResponseDto<DashboardStatsDto>.ErrorResult("Có lỗi xảy ra khi lấy thống kê"));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Documents.Application.DTOs;

namespace ManagementSystem.Modules.Documents.Presentation.Controllers;

[ApiController]
[Route("api/tags")]
[Authorize]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;
    private readonly ILogger<TagsController> _logger;

    public TagsController(ITagService tagService, ILogger<TagsController> logger)
    {
        _tagService = tagService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<List<TagDto>>>> GetAllAsync()
    {
        try
        {
            var tags = await _tagService.GetAllAsync();
            return Ok(ApiResponseDto<List<TagDto>>.SuccessResult(tags, "Lấy danh sách thẻ thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tags");
            return StatusCode(500, ApiResponseDto<List<TagDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách thẻ", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<TagDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag is null)
            {
                return NotFound(ApiResponseDto<TagDto>.ErrorResult("Không tìm thấy thẻ", 404));
            }
            return Ok(ApiResponseDto<TagDto>.SuccessResult(tag, "Lấy thông tin thẻ thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tag {TagId}", id);
            return StatusCode(500, ApiResponseDto<TagDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin thẻ", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<TagDto>>> CreateAsync([FromBody] CreateTagDto dto)
    {
        try
        {
            var tag = await _tagService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = tag.Id },
                ApiResponseDto<TagDto>.SuccessResult(tag, "Tạo thẻ thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tag");
            return StatusCode(500, ApiResponseDto<TagDto>.ErrorResult("Có lỗi xảy ra khi tạo thẻ", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<TagDto>>> UpdateAsync(Guid id, [FromBody] UpdateTagDto dto)
    {
        try
        {
            var tag = await _tagService.UpdateAsync(id, dto);
            if (tag is null)
            {
                return NotFound(ApiResponseDto<TagDto>.ErrorResult("Không tìm thấy thẻ", 404));
            }
            return Ok(ApiResponseDto<TagDto>.SuccessResult(tag, "Cập nhật thẻ thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tag {TagId}", id);
            return StatusCode(500, ApiResponseDto<TagDto>.ErrorResult("Có lỗi xảy ra khi cập nhật thẻ", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _tagService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy thẻ", 404));
            }
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa thẻ thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tag {TagId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa thẻ", 500));
        }
    }
}

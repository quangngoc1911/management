using System.Security.Claims;
using ManagementSystem.DTOs.Category;
using ManagementSystem.DTOs.Common;
using ManagementSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(
        ICategoryService categoryService,
        ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
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

    /// <summary>
    /// Get all categories
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponseDto<List<CategoryDto>>>> GetAllAsync()
    {
        try
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(ApiResponseDto<List<CategoryDto>>.SuccessResult(categories, "Lấy danh sách danh mục thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting categories");
            return StatusCode(500, ApiResponseDto<List<CategoryDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách danh mục"));
        }
    }

    /// <summary>
    /// Get category by slug
    /// </summary>
    [HttpGet("{slug}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponseDto<CategoryDto>>> GetBySlugAsync(string slug)
    {
        try
        {
            var category = await _categoryService.GetBySlugAsync(slug);
            if (category == null)
            {
                return NotFound(ApiResponseDto<CategoryDto>.ErrorResult("Không tìm thấy danh mục"));
            }
            return Ok(ApiResponseDto<CategoryDto>.SuccessResult(category, "Lấy thông tin danh mục thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting category by slug {CategorySlug}", slug);
            return StatusCode(500, ApiResponseDto<CategoryDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin danh mục"));
        }
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<CategoryDto>>> CreateAsync(
        [FromBody] CreateCategoryDto dto)
    {
        try
        {
            dto.CreatedBy = GetCurrentUserId(); // Set CreatedBy from token
            var category = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetBySlugAsync),
                new { slug = category.Slug },
                ApiResponseDto<CategoryDto>.SuccessResult(category, "Tạo danh mục thành công")
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category");
            return StatusCode(500, ApiResponseDto<CategoryDto>.ErrorResult("Có lỗi xảy ra khi tạo danh mục"));
        }
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<CategoryDto>>> UpdateAsync(
        Guid id,
        [FromBody] CreateCategoryDto dto)
    {
        try
        {
            dto.UpdatedBy = GetCurrentUserId(); // Set UpdatedBy from token
            var category = await _categoryService.UpdateAsync(id, dto);
            if (category == null)
            {
                return NotFound(ApiResponseDto<CategoryDto>.ErrorResult("Không tìm thấy danh mục"));
            }
            return Ok(ApiResponseDto<CategoryDto>.SuccessResult(category, "Cập nhật danh mục thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category {CategoryId}", id);
            return StatusCode(500, ApiResponseDto<CategoryDto>.ErrorResult("Có lỗi xảy ra khi cập nhật danh mục"));
        }
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy danh mục"));
            }
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa danh mục thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category {CategoryId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa danh mục"));
        }
    }

    /// <summary>
    /// Move a category to a new parent
    /// </summary>
    [HttpPut("{id:guid}/move")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<bool>>> MoveCategoryAsync(
        Guid id,
        [FromQuery] Guid? newParentId)
    {
        try
        {
            var result = await _categoryService.MoveAsync(id, newParentId);
            if (!result)
            {
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy danh mục hoặc danh mục cha mới không hợp lệ"));
            }
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Di chuyển danh mục thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error moving category {CategoryId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi di chuyển danh mục"));
        }
    }
}

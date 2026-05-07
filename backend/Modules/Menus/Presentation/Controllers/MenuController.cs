using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Menus.Application.DTOs;
using ManagementSystem.Application.DTOs.Common;

namespace ManagementSystem.Modules.Menus.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;
    private readonly ILogger<MenuController> _logger;

    public MenuController(IMenuService menuService, ILogger<MenuController> logger)
    {
        _menuService = menuService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet("tree")]
    public async Task<ActionResult<ApiResponseDto<List<MenuDto>>>> GetTree()
    {
        try
        {
            var result = await _menuService.GetMenuTreeAsync();
            return Ok(ApiResponseDto<List<MenuDto>>.SuccessResult(result, "Lấy danh sách menu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting menu tree");
            return StatusCode(500, ApiResponseDto<List<MenuDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách menu"));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<MenuDto>>> GetById(Guid id)
    {
        try
        {
            var result = await _menuService.GetByIdAsync(id);
            if (result == null)
                return NotFound(ApiResponseDto<MenuDto>.ErrorResult("Không tìm thấy menu"));

            return Ok(ApiResponseDto<MenuDto>.SuccessResult(result, "Lấy thông tin menu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting menu item {MenuId}", id);
            return StatusCode(500, ApiResponseDto<MenuDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin menu"));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<MenuDto>>> Create([FromBody] CreateMenuDto dto)
    {
        try
        {
            var result = await _menuService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponseDto<MenuDto>.SuccessResult(result, "Tạo menu thành công"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponseDto<MenuDto>.ErrorResult(ex.Message));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponseDto<MenuDto>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating menu");
            return StatusCode(500, ApiResponseDto<MenuDto>.ErrorResult("Có lỗi xảy ra khi tạo menu"));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<MenuDto>>> Update(Guid id, [FromBody] CreateMenuDto dto)
    {
        try
        {
            var result = await _menuService.UpdateAsync(id, dto);
            if (result == null)
                return NotFound(ApiResponseDto<MenuDto>.ErrorResult("Không tìm thấy menu"));

            return Ok(ApiResponseDto<MenuDto>.SuccessResult(result, "Cập nhật menu thành công"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponseDto<MenuDto>.ErrorResult(ex.Message));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponseDto<MenuDto>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating menu {MenuId}", id);
            return StatusCode(500, ApiResponseDto<MenuDto>.ErrorResult("Có lỗi xảy ra khi cập nhật menu"));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> Delete(Guid id)
    {
        try
        {
            var result = await _menuService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy menu"));

            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa menu thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting menu {MenuId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa menu"));
        }
    }
}

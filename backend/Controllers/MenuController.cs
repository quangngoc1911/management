using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyAPI.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class MenusController : ControllerBase
{
    private readonly IMenuService _menuService;
    public MenusController(IMenuService menuService) => _menuService = menuService;

    [AllowAnonymous]     // ← public, không cần đăng nhập
    [HttpGet("tree")]
    public async Task<IActionResult> GetTree()
    {
        var result = await _menuService.GetMenuTreeAsync();
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMenuDto dto)
    {
        var result = await _menuService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetTree), new { id = result.Id }, result);
    }
}
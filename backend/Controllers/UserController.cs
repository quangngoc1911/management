using Microsoft.AspNetCore.Mvc;
using MyAPI.DTOs;
using MyAPI.Interfaces;

namespace MyAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _service.GetUsers();

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserDTO dto)
    {
        var user = await _service.CreateUser(dto);

        return Ok(user);
    }
}
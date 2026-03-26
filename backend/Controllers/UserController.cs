using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MyAPI.DTOs;
using MyAPI.Interfaces;


namespace MyAPI.Controllers;

[Authorize]
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
        Console.WriteLine($">>> IsAuthenticated: {User.Identity?.IsAuthenticated}");
        Console.WriteLine($">>> AuthType: {User.Identity?.AuthenticationType}");
        Console.WriteLine($">>> Name: {User.Identity?.Name}");
        Console.WriteLine($">>> Token: {Request.Headers["Authorization"]}");

        var users = await _service.GetUsers();

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserDto dto)
    {
        var user = await _service.CreateUser(dto);

        return Ok(user);
    }
}
using MyAPI.DTOs;
using MyAPI.Models;

namespace MyAPI.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetUsers();
    Task<UserDto> CreateUser(UserDto dto);
    Task<UserDto> RegisterAsync(RegisterRequest request);
}
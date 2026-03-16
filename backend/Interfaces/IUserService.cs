using MyAPI.DTOs;
using MyAPI.Models;

namespace MyAPI.Interfaces;

public interface IUserService
{
    Task<List<User>> GetUsers();

    Task<User> CreateUser(UserDTO dto);
}
using MyAPI.DTOs;
using MyAPI.Models;

namespace MyAPI.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<List<User>> GetPagedAsync(int skip, int take);
    Task<int> CountAsync();
    Task<User> Add(User user);
    Task<User> AddAsync(User user);
}
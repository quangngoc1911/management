using MyAPI.Models;

namespace MyAPI.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAll();

    Task<User> Add(User user);
}
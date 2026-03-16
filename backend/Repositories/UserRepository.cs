using MyAPI.Data;
using MyAPI.Interfaces;
using MyAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MyAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> Add(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}
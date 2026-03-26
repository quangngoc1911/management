using Microsoft.EntityFrameworkCore;

using MyAPI.Data;
using MyAPI.DTOs;
using MyAPI.Interfaces;
using MyAPI.Models;

namespace MyAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> Add(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user; // ✅ trả về
    }

    public async Task<List<User>> GetPagedAsync(int skip, int take)
    {
        return await _context.Users
            .OrderBy(u => u.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}
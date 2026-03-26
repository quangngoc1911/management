using Microsoft.EntityFrameworkCore;

using MyAPI.Data;
using MyAPI.Interfaces;
using MyAPI.Models;

namespace MyAPI.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly AppDbContext _context;
    public MenuRepository(AppDbContext context) => _context = context;

    public async Task<List<Menu>> GetMenuTreeAsync()
    {
        return await _context.Menus
            .Include(m => m.Children)
            .Where(m => m.ParentId == null)
            .ToListAsync();
    }
    public async Task<Menu?> GetByIdAsync(int id)
    {
        return await _context.Menus.FindAsync(id);
    }

    public async Task AddAsync(Menu menu)
    {
        await _context.Menus.AddAsync(menu);
        await _context.SaveChangesAsync();
    }
}
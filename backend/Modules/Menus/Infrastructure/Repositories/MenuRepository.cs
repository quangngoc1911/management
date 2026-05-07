using Microsoft.EntityFrameworkCore;

using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Menus.Domain.Entities;

namespace ManagementSystem.Modules.Menus.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly ApplicationDbContext _context;
    public MenuRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<Menu>> GetMenuTreeAsync()
    {
        return await _context.Menus
            .Include(m => m.Children)
            .Where(m => m.ParentId == null && !m.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Menu?> GetByIdAsync(Guid id)
    {
        return await _context.Menus
            .Include(m => m.Children)
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
    }

    public async Task CreateAsync(Menu menu)
    {
        await _context.Menus.AddAsync(menu);
    }

    public void Update(Menu menu)
    {
        _context.Menus.Update(menu);
    }

    public void Delete(Menu menu)
    {
        _context.Menus.Remove(menu);
    }
}



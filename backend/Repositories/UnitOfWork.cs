using ManagementSystem.Interfaces;
using ManagementSystem.Data;

namespace ManagementSystem.Repositories;

/// <summary>
/// Unit of Work implementation for managing database operations
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDocumentRepository? _documents;
    private ICategoryRepository? _categories;
    private IUserRepository? _users;
    private IMenuRepository? _menus;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IDocumentRepository Documents => _documents ??= new DocumentRepository(_context);
    
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
    
    public IUserRepository Users => _users ??= new UserRepository(_context);
    
    public IMenuRepository Menus => _menus ??= new MenuRepository(_context);
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}
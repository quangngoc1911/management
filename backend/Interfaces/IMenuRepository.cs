using MyAPI.Models;

namespace MyAPI.Interfaces;

public interface IMenuRepository
{
    Task<List<Menu>> GetMenuTreeAsync();
    Task<Menu?> GetByIdAsync(int id);       // ← cần để validate ParentId
    Task AddAsync(Menu menu);
}
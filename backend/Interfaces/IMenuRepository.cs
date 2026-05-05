using ManagementSystem.Entities;

namespace ManagementSystem.Interfaces;

public interface IMenuRepository
{
    Task<List<Menu>> GetMenuTreeAsync();
    Task<Menu?> GetByIdAsync(Guid id);
    Task CreateAsync(Menu menu);
    void Update(Menu menu);
    void Delete(Menu menu);
}

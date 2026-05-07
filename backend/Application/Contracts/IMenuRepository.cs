using ManagementSystem.Modules.Menus.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IMenuRepository
{
    Task<List<Menu>> GetMenuTreeAsync();
    Task<Menu?> GetByIdAsync(Guid id);
    Task CreateAsync(Menu menu);
    void Update(Menu menu);
    void Delete(Menu menu);
}
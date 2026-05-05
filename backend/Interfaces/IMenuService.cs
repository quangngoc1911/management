using ManagementSystem.DTOs.Menu;

namespace ManagementSystem.Interfaces;

public interface IMenuService
{
    Task<List<MenuDto>> GetMenuTreeAsync();
    Task<MenuDto?> GetByIdAsync(Guid id);
    Task<MenuDto> CreateAsync(CreateMenuDto dto);
    Task<MenuDto?> UpdateAsync(Guid id, CreateMenuDto dto);
    Task<bool> DeleteAsync(Guid id);
}

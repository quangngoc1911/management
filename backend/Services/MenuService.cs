using ManagementSystem.DTOs.Menu;
using ManagementSystem.Entities;
using ManagementSystem.Interfaces;

namespace ManagementSystem.Services;

public class MenuService : IMenuService
{
    private readonly IUnitOfWork _unitOfWork;

    public MenuService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<MenuDto>> GetMenuTreeAsync()
    {
        var menus = await _unitOfWork.Menus.GetMenuTreeAsync();
        return menus.Select(MapToDto).ToList();
    }

    public async Task<MenuDto?> GetByIdAsync(Guid id)
    {
        var menu = await _unitOfWork.Menus.GetByIdAsync(id);
        if (menu == null) return null;
        return MapToDto(menu);
    }

    public async Task<MenuDto> CreateAsync(CreateMenuDto dto)
    {
        if (dto.ParentId.HasValue)
        {
            var parent = await _unitOfWork.Menus.GetByIdAsync(dto.ParentId.Value);
            if (parent == null)
                throw new KeyNotFoundException($"Không tìm thấy menu cha Id={dto.ParentId}");
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Tên menu không được để trống");

        var menu = new Menu
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Path = dto.Path,
            Icon = dto.Icon,
            SortOrder = dto.SortOrder,
            IsActive = dto.IsActive,
            ParentId = dto.ParentId
        };

        await _unitOfWork.Menus.CreateAsync(menu);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(menu);
    }

    public async Task<MenuDto?> UpdateAsync(Guid id, CreateMenuDto dto)
    {
        var menu = await _unitOfWork.Menus.GetByIdAsync(id);
        if (menu == null) return null;

        if (dto.ParentId.HasValue && dto.ParentId.Value != menu.ParentId)
        {
            if (dto.ParentId.Value == id)
                throw new ArgumentException("Menu không thể là cha của chính nó");

            var parent = await _unitOfWork.Menus.GetByIdAsync(dto.ParentId.Value);
            if (parent == null)
                throw new KeyNotFoundException($"Không tìm thấy menu cha Id={dto.ParentId}");
        }

        menu.Name = dto.Name;
        menu.Path = dto.Path;
        menu.Icon = dto.Icon;
        menu.SortOrder = dto.SortOrder;
        menu.IsActive = dto.IsActive;
        menu.ParentId = dto.ParentId;

        _unitOfWork.Menus.Update(menu);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(menu);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var menu = await _unitOfWork.Menus.GetByIdAsync(id);
        if (menu == null) return false;

        _unitOfWork.Menus.Delete(menu);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private static MenuDto MapToDto(Menu m) => new()
    {
        Id = m.Id,
        Name = m.Name,
        Path = m.Path,
        Icon = m.Icon,
        SortOrder = m.SortOrder,
        IsActive = m.IsActive,
        ParentId = m.ParentId,
        Children = m.Children?.Select(MapToDto).ToList() ?? new List<MenuDto>()
    };
}

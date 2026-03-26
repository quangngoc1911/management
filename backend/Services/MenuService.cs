using AutoMapper;

using MyAPI.DTOs;
using MyAPI.Interfaces;
using MyAPI.Models;

namespace MyAPI.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepo;
    public MenuService(IMenuRepository menuRepo) => _menuRepo = menuRepo;

    public async Task<List<MenuDto>> GetMenuTreeAsync()
    {
        var menus = await _menuRepo.GetMenuTreeAsync();
        return menus.Select(MapToDto).ToList();
    }

    private static MenuDto MapToDto(Menu m) => new()
    {
        Id = m.Id,
        Name = m.Name,
        Path = m.Path,
        Icon = m.Icon,
        Order = m.Order,
        Children = m.Children.Select(MapToDto).ToList()  // đệ quy
    };
    public async Task<MenuDto> CreateAsync(CreateMenuDto dto)
    {
        if (dto.ParentId.HasValue)
        {
            var parent = await _menuRepo.GetByIdAsync(dto.ParentId.Value);
            if (parent == null)
                throw new KeyNotFoundException($"Không tìm thấy menu cha Id={dto.ParentId}");
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Tên menu không được để trống");

        if (dto.ParentId > 0)
        {
            var parent = await _menuRepo.GetByIdAsync(dto.ParentId.Value);
            if (parent == null)
                throw new KeyNotFoundException($"Không tìm thấy menu cha Id={dto.ParentId}");
        }

        var menu = new Menu
        {
            Name = dto.Name,
            // Slug = dto.Slug,
            Path = dto.Path,
            Icon = dto.Icon,
            Order = dto.Order,
            IsVisible = dto.IsVisible,
            // RequiredRole = dto.RequiredRole,
            ParentId = dto.ParentId
        };

        await _menuRepo.AddAsync(menu);

        return new MenuDto
        {
            Id = menu.Id,
            Name = menu.Name,
            Path = menu.Path,
            Icon = menu.Icon,
            Order = menu.Order,
            Children = new()
        };
    }
}
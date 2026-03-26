using MyAPI.DTOs;
using MyAPI.Models;

namespace MyAPI.Interfaces;

public interface IMenuService
{
    Task<List<MenuDto>> GetMenuTreeAsync();
    Task<MenuDto> CreateAsync(CreateMenuDto dto);

}
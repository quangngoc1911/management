using ManagementSystem.Modules.Categories.Application.DTOs;
using ManagementSystem.Application.DTOs.Common;

namespace ManagementSystem.Application.Contracts;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetBySlugAsync(string slug);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    Task<CategoryDto?> UpdateAsync(Guid id, CreateCategoryDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> MoveAsync(Guid id, Guid? newParentId);
}
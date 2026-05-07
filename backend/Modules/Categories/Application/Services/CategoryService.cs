using ManagementSystem.Modules.Categories.Application.DTOs;
using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Categories.Domain.Entities;
using ManagementSystem.Application.DTOs.Common;

namespace ManagementSystem.Modules.Categories.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTime _dateTime;

    public CategoryService(IDateTime dateTime,IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Slug = c.Slug,
            Icon = c.Icon,
            CoverImageUrl = c.CoverImageUrl,
            ParentId = c.ParentId,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();
    }

    public async Task<CategoryDto?> GetBySlugAsync(string slug)
    {
        var category = await _unitOfWork.Categories.GetBySlugAsync(slug);
        if (category == null) return null;

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Slug = category.Slug,
            Icon = category.Icon,
            CoverImageUrl = category.CoverImageUrl,
            ParentId = category.ParentId,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            Slug = dto.Slug?.ToLower().Trim().Replace(" ", "-") ?? string.Empty,
            Icon = dto.Icon,
            CoverImageUrl = dto.CoverImageUrl,
            ParentId = dto.ParentId,
            CreatedAt = _dateTime.UtcNow,
            CreatedBy = dto.CreatedBy
        };
        await _unitOfWork.Categories.CreateAsync(category);
        await _unitOfWork.SaveChangesAsync();
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Slug = category.Slug,
            Icon = category.Icon,
            CoverImageUrl = category.CoverImageUrl,
            ParentId = category.ParentId,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    public async Task<CategoryDto?> UpdateAsync(Guid id, CreateCategoryDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return null;

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.Slug = dto.Slug?.ToLower().Trim().Replace(" ", "-") ?? string.Empty;
        category.Icon = dto.Icon;
        category.CoverImageUrl = dto.CoverImageUrl;
        category.ParentId = dto.ParentId;
        category.UpdatedAt = _dateTime.UtcNow;
        category.UpdatedBy = dto.UpdatedBy;

        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();
        
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Slug = category.Slug,
            Icon = category.Icon,
            CoverImageUrl = category.CoverImageUrl,
            ParentId = category.ParentId,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return false;

        await _unitOfWork.Categories.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MoveAsync(Guid id, Guid? newParentId)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return false;

        category.ParentId = newParentId;
        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

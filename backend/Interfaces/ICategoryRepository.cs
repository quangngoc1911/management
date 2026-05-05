using ManagementSystem.Entities;

namespace ManagementSystem.Interfaces;

/// <summary>
/// Repository interface for Category operations
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Get all categories
    /// </summary>
    Task<IEnumerable<Category>> GetAllAsync();
    
    /// <summary>
    /// Get category by ID
    /// </summary>
    Task<Category?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get category by slug
    /// </summary>
    Task<Category?> GetBySlugAsync(string slug);
    
    /// <summary>
    /// Create a new category
    /// </summary>
    Task CreateAsync(Category category);
    
    /// <summary>
    /// Update a category
    /// </summary>
    void Update(Category category);
    
    /// <summary>
    /// Delete a category
    /// </summary>
    Task DeleteAsync(Guid id);
}
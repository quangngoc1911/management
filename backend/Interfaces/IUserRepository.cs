using ManagementSystem.Entities;

namespace ManagementSystem.Interfaces;

/// <summary>
/// Repository interface for User operations (MyAPI namespace)
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Get all users
    /// </summary>
    Task<IEnumerable<User>> GetAllAsync();
    
    /// <summary>
    /// Get user by ID
    /// </summary>
    Task<User?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get user by email
    /// </summary>
    Task<User?> GetByEmailAsync(string email);
    
    /// <summary>
    /// Create a new user
    /// </summary>
    Task CreateAsync(User user);
    
    /// <summary>
    /// Update a user
    /// </summary>
    void Update(User user);
    
    /// <summary>
    /// Delete a user
    /// </summary>
    void Delete(User user);
    
    /// <summary>
    /// Count users
    /// </summary>
    Task<int> CountAsync();
    
    /// <summary>
    /// Get users with pagination
    /// </summary>
    Task<IEnumerable<User>> GetPagedAsync(int skip, int take);
    
    /// <summary>
    /// Check if email exists
    /// </summary>
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
}

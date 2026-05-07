namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Unit of Work pattern interface for managing database operations
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Document repository
    /// </summary>
    IDocumentRepository Documents { get; }
    
    /// <summary>
    /// Category repository
    /// </summary>
    ICategoryRepository Categories { get; }
    
    /// <summary>
    /// User repository
    /// </summary>
    IUserRepository Users { get; }
    

    /// <summary>
    /// Menu repository
    /// </summary>
    IMenuRepository Menus { get; }

    /// <summary>
    /// Save all changes
    /// </summary>
    Task<int> SaveChangesAsync();
}
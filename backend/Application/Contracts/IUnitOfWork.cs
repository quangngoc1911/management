namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Unit of Work pattern interface for managing database operations
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IDocumentRepository Documents { get; }

    ICategoryRepository Categories { get; }

    IUserRepository Users { get; }

    Task<int> SaveChangesAsync();
}

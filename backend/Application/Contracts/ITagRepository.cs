using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Repository interface for Tag operations.
/// </summary>
public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetAllAsync();
    Task<Tag?> GetByIdAsync(Guid id);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null);
    Task CreateAsync(Tag tag);
    void Update(Tag tag);
    Task<bool> DeleteAsync(Guid id);
}

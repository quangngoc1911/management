using ManagementSystem.Modules.Documents.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Application service for managing tags.
/// </summary>
public interface ITagService
{
    Task<List<TagDto>> GetAllAsync();
    Task<TagDto?> GetByIdAsync(Guid id);
    Task<TagDto> CreateAsync(CreateTagDto dto);
    Task<TagDto?> UpdateAsync(Guid id, UpdateTagDto dto);
    Task<bool> DeleteAsync(Guid id);
}

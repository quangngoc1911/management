using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Tag operations.
/// Soft-deleted rows are excluded automatically by the global query filter.
/// </summary>
public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _context.Tags
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<Tag?> GetByIdAsync(Guid id)
    {
        return await _context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null)
    {
        var query = _context.Tags.Where(t => t.Slug == slug);
        if (excludeId.HasValue)
        {
            query = query.Where(t => t.Id != excludeId.Value);
        }
        return await query.AnyAsync();
    }

    public async Task CreateAsync(Tag tag)
    {
        await _context.Tags.AddAsync(tag);
    }

    public void Update(Tag tag)
    {
        _context.Tags.Update(tag);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
        if (tag == null)
        {
            return false;
        }

        tag.IsDeleted = true;
        _context.Tags.Update(tag);
        return true;
    }
}

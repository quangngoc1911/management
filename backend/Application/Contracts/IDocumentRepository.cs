using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Documents.Application.DTOs;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Repository interface for Document operations
/// </summary>
public interface IDocumentRepository
{
    /// <summary>
    /// Get all documents with pagination and filtering
    /// </summary>
    Task<PaginatedResultDto<DocumentListDto>> GetAllAsync(DocumentQueryDto query);
    
    /// <summary>
    /// Get document by ID
    /// </summary>
    Task<DocumentDto?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Get document entity by ID (for update operations)
    /// </summary>
    Task<Document?> GetEntityByIdAsync(Guid id);
    
    /// <summary>
    /// Create a new document
    /// </summary>
    Task CreateAsync(Document document);
    
    /// <summary>
    /// Update a document
    /// </summary>
    void Update(Document document);
    
    /// <summary>
    /// Delete a document
    /// </summary>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Get dashboard statistics
    /// </summary>
    Task<DashboardStatsDto> GetDashboardStatsAsync();
}


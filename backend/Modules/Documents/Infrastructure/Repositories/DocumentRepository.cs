using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Documents.Application.DTOs;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Document operations
/// </summary>
public class DocumentRepository : IDocumentRepository
{
    private readonly ApplicationDbContext _context;

    public DocumentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResultDto<DocumentListDto>> GetAllAsync(DocumentQueryDto query)
    {
        var documentsQuery = _context.Documents
            .Include(d => d.Category)
            .Include(d => d.CreatedByUser)
            .Include(d => d.Fields)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            documentsQuery = documentsQuery.Where(d =>
                d.Title.ToLower().Contains(search) ||
                (d.Description != null && d.Description.ToLower().Contains(search)) ||
                (d.DocumentNumber != null && d.DocumentNumber.ToLower().Contains(search)));
        }

        if (!string.IsNullOrWhiteSpace(query.CategoryId) && Guid.TryParse(query.CategoryId, out var categoryId))
        {
            documentsQuery = documentsQuery.Where(d => d.CategoryId == categoryId);
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            documentsQuery = documentsQuery.Where(d => d.Status == query.Status);
        }

        if (query.FromDate.HasValue)
        {
            documentsQuery = documentsQuery.Where(d => d.CreatedAt >= query.FromDate);
        }
        if (query.ToDate.HasValue)
        {
            documentsQuery = documentsQuery.Where(d => d.CreatedAt <= query.ToDate);
        }

        documentsQuery = query.SortBy?.ToLower() switch
        {
            "title" => query.IsDescending
                ? documentsQuery.OrderByDescending(d => d.Title)
                : documentsQuery.OrderBy(d => d.Title),
            "documentnumber" => query.IsDescending
                ? documentsQuery.OrderByDescending(d => d.DocumentNumber)
                : documentsQuery.OrderBy(d => d.DocumentNumber),
            "status" => query.IsDescending
                ? documentsQuery.OrderByDescending(d => d.Status)
                : documentsQuery.OrderBy(d => d.Status),
            _ => query.IsDescending
                ? documentsQuery.OrderByDescending(d => d.CreatedAt)
                : documentsQuery.OrderBy(d => d.CreatedAt)
        };

        var totalCount = await documentsQuery.CountAsync();
        var documents = await documentsQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(d => new DocumentListDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                DocumentNumber = d.DocumentNumber,
                CategoryId = d.CategoryId,
                CategoryName = d.Category != null ? d.Category.Name : string.Empty,
                IssueDate = d.IssueDate,
                ExpiryDate = d.ExpiryDate,
                Status = d.Status,
                FilePath = d.FilePath,
                FileName = d.FileName,
                FileSize = d.FileSize,
                FileType = d.FileType,
                CreatedByUserId = d.CreatedByUserId,
                CreatedByUserName = d.CreatedByUser != null ? d.CreatedByUser.Name : string.Empty,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
                Fields = d.Fields.Select(f => new DocumentFieldDto
                {
                    Id = f.Id,
                    FieldName = f.FieldName,
                    FieldValue = f.FieldValue,
                    FieldType = f.FieldType ?? string.Empty,
                    SortOrder = f.SortOrder
                }).ToList()
            })
            .ToListAsync();

        return new PaginatedResultDto<DocumentListDto>
        {
            Items = documents,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<DocumentDto?> GetByIdAsync(Guid id)
    {
        var document = await _context.Documents
            .Include(d => d.Category)
            .Include(d => d.CreatedByUser)
            .Include(d => d.Fields)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);

        if (document == null)
            return null;

        return MapToDto(document);
    }

    public async Task<Document?> GetEntityByIdAsync(Guid id)
    {
        return await _context.Documents
            .Include(d => d.Fields)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task CreateAsync(Document document)
    {
        await _context.Documents.AddAsync(document);
    }

    public void Update(Document document)
    {
        _context.Documents.Update(document);
    }

    public async Task DeleteAsync(Guid id)
    {
        var document = await _context.Documents.FindAsync(id);
        if (document != null)
        {
            _context.Documents.Remove(document);
        }
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        var totalDocuments = await _context.Documents.CountAsync();
        var activeDocuments = await _context.Documents.CountAsync(d => d.Status == "Active");
        var expiredDocuments = await _context.Documents.CountAsync(d => d.ExpiryDate.HasValue && d.ExpiryDate < DateTime.UtcNow);
        var totalCategories = await _context.Categories.CountAsync();
        var totalUsers = await _context.Users.CountAsync();

        return new DashboardStatsDto
        {
            TotalDocuments = totalDocuments,
            ActiveDocuments = activeDocuments,
            ExpiredDocuments = expiredDocuments,
            TotalCategories = totalCategories,
            TotalUsers = totalUsers
        };
    }

    private DocumentDto MapToDto(Document document)
    {
        return new DocumentDto
        {
            Id = document.Id,
            Title = document.Title,
            Description = document.Description,
            DocumentNumber = document.DocumentNumber ?? string.Empty,
            CategoryId = document.CategoryId.ToString(),
            CategoryName = document.Category?.Name ?? string.Empty,
            IssueDate = document.IssueDate ?? DateTime.MinValue,
            ExpiryDate = document.ExpiryDate,
            Status = document.Status ?? string.Empty,
            FilePath = document.FilePath,
            FileName = document.FileName,
            FileSize = document.FileSize,
            FileType = document.FileType,
            CreatedByUserId = document.CreatedByUserId,
            CreatedByUserName = document.CreatedByUser?.Name ?? string.Empty,
            CreatedAt = document.CreatedAt,
            UpdatedByUserId = document.UpdatedByUserId,
            UpdatedByUserName = document.UpdatedByUser?.Name,
            UpdatedAt = document.UpdatedAt,
            Fields = document.Fields.Select(f => new DocumentFieldDto
            {
                Id = f.Id,
                FieldName = f.FieldName,
                FieldValue = f.FieldValue,
                FieldType = f.FieldType ?? string.Empty,
                SortOrder = f.SortOrder
            }).ToList()
        };
    }
}



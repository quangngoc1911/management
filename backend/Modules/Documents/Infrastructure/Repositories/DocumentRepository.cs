using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Documents.Application.DTOs;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Infrastructure.Repositories;

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
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            documentsQuery = documentsQuery.Where(d =>
                d.Title.ToLower().Contains(search) ||
                (d.Summary != null && d.Summary.ToLower().Contains(search)) ||
                d.Slug.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(query.CategoryId) && Guid.TryParse(query.CategoryId, out var categoryId))
        {
            documentsQuery = documentsQuery.Where(d => d.CategoryId == categoryId);
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
            "viewcount" => query.IsDescending
                ? documentsQuery.OrderByDescending(d => d.ViewCount)
                : documentsQuery.OrderBy(d => d.ViewCount),
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
                Slug = d.Slug,
                Summary = d.Summary,
                ContentType = d.ContentType,
                ThumbnailUrl = d.ThumbnailUrl,
                CategoryId = d.CategoryId,
                CategoryName = d.Category != null ? d.Category.Name : string.Empty,
                MemberId = d.MemberId,
                CreatedByUserId = d.CreatedByUserId,
                CreatedByUserName = d.CreatedByUser != null ? (d.CreatedByUser.UserName ?? string.Empty) : string.Empty,
                IsPublished = d.IsPublished,
                PublishedAt = d.PublishedAt,
                ViewCount = d.ViewCount,
                CreatedAt = d.CreatedAt ?? DateTime.MinValue,
                UpdatedAt = d.UpdatedAt
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
            .Include(d => d.Member)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);

        if (document == null)
            return null;

        return MapToDto(document);
    }

    public async Task<Document?> GetEntityByIdAsync(Guid id)
    {
        return await _context.Documents.FirstOrDefaultAsync(d => d.Id == id);
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
        var publishedDocuments = await _context.Documents.CountAsync(d => d.IsPublished);
        var totalCategories = await _context.Categories.CountAsync();
        var totalUsers = await _context.Users.CountAsync();

        return new DashboardStatsDto
        {
            TotalDocuments = totalDocuments,
            ActiveDocuments = publishedDocuments,
            ExpiredDocuments = 0,
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
            Slug = document.Slug,
            Summary = document.Summary,
            ContentType = document.ContentType,
            Content = document.Content,
            FileId = document.FileId,
            ThumbnailUrl = document.ThumbnailUrl,
            CategoryId = document.CategoryId,
            CategoryName = document.Category?.Name ?? string.Empty,
            MemberId = document.MemberId,
            MemberName = document.Member?.FullName,
            CreatedByUserId = document.CreatedByUserId,
            CreatedByUserName = document.CreatedByUser?.UserName ?? string.Empty,
            IsPublished = document.IsPublished,
            PublishedAt = document.PublishedAt,
            ViewCount = document.ViewCount,
            SortOrder = document.SortOrder,
            Version = document.Version,
            CreatedAt = document.CreatedAt ?? DateTime.MinValue,
            UpdatedAt = document.UpdatedAt
        };
    }
}

using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Documents.Application.DTOs;
using ManagementSystem.Modules.Documents.Domain.Entities;
using ManagementSystem.Application.Contracts;

namespace ManagementSystem.Modules.Documents.Application.Services;

public class DocumentService : IDocumentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDateTime _dateTime;

    public DocumentService(IDateTime dateTime, IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<DocumentListDto>> GetAllAsync(DocumentQueryDto query)
    {
        return await _unitOfWork.Documents.GetAllAsync(query);
    }

    public async Task<DocumentDto?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.Documents.GetByIdAsync(id);
    }

    public async Task<DocumentDto> CreateAsync(CreateDocumentDto dto, Guid userId)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
        if (category == null)
            throw new ArgumentException("Danh mục không hợp lệ");

        var document = new Document
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Slug = dto.Slug,
            Summary = dto.Summary,
            ContentType = dto.ContentType,
            Content = dto.Content,
            FileId = dto.FileId,
            ThumbnailUrl = dto.ThumbnailUrl,
            CategoryId = dto.CategoryId,
            MemberId = dto.MemberId,
            IsPublished = dto.IsPublished,
            PublishedAt = dto.IsPublished ? _dateTime.UtcNow : null,
            SortOrder = dto.SortOrder,
            Version = 1,
            CreatedByUserId = userId,
            CreatedAt = _dateTime.UtcNow
        };

        await _unitOfWork.Documents.CreateAsync(document);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(document.Id)
            ?? throw new InvalidOperationException($"Document {document.Id} was saved but could not be retrieved.");
    }

    public async Task<DocumentDto?> UpdateAsync(UpdateDocumentDto dto, Guid userId)
    {
        var document = await _unitOfWork.Documents.GetEntityByIdAsync(dto.Id);
        if (document == null)
            return null;

        if (dto.Title != null) document.Title = dto.Title;
        if (dto.Slug != null) document.Slug = dto.Slug;
        if (dto.Summary != null) document.Summary = dto.Summary;
        if (dto.ContentType != null) document.ContentType = dto.ContentType;
        if (dto.Content != null) document.Content = dto.Content;
        if (dto.ThumbnailUrl != null) document.ThumbnailUrl = dto.ThumbnailUrl;
        if (dto.CategoryId.HasValue) document.CategoryId = dto.CategoryId.Value;
        if (dto.MemberId.HasValue) document.MemberId = dto.MemberId;
        if (dto.SortOrder.HasValue) document.SortOrder = dto.SortOrder.Value;

        if (dto.IsPublished.HasValue && dto.IsPublished.Value != document.IsPublished)
        {
            document.IsPublished = dto.IsPublished.Value;
            document.PublishedAt = dto.IsPublished.Value ? _dateTime.UtcNow : (DateTime?)null;
        }

        if (dto.RemoveFile)
        {
            document.FileId = null;
        }
        else if (dto.FileId.HasValue)
        {
            document.FileId = dto.FileId;
        }

        document.UpdatedAt = _dateTime.UtcNow;
        document.Version += 1;

        _unitOfWork.Documents.Update(document);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(document.Id);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var document = await _unitOfWork.Documents.GetEntityByIdAsync(id);
        if (document == null)
            return false;

        await _unitOfWork.Documents.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public Task<(byte[] fileBytes, string fileName, string contentType)?> DownloadFileAsync(Guid id)
    {
        // File download now goes through DocumentFile entity (FileId reference).
        // Will be reimplemented when files module gets its own service.
        return Task.FromResult<(byte[], string, string)?>(null);
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        return await _unitOfWork.Documents.GetDashboardStatsAsync();
    }
}

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

    public DocumentService(IDateTime dateTime,IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
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
            Description = dto.Description,
            DocumentNumber = dto.DocumentNumber,
            CategoryId = dto.CategoryId,
            IssueDate = dto.IssueDate,
            ExpiryDate = dto.ExpiryDate,
            Status = dto.Status ?? "Draft",
            CreatedByUserId = userId,
            CreatedAt = _dateTime.UtcNow
        };

        if (dto.File != null)
        {
            var fileResult = await _fileStorageService.UploadAsync(dto.File, "documents");
            document.FilePath = fileResult.FilePath;
            document.FileName = fileResult.OriginalFileName;
            document.FileSize = fileResult.FileSize;
            document.FileType = fileResult.ContentType;
        }

        if (dto.Fields != null)
        {
            foreach (var field in dto.Fields)
            {
                document.Fields.Add(new DocumentField
                {
                    Id = Guid.NewGuid(),
                    DocumentId = document.Id,
                    FieldName = field.FieldName,
                    FieldValue = field.FieldValue,
                    FieldType = field.FieldType,
                    SortOrder = field.SortOrder
                });
            }
        }

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
        document.Description = dto.Description;
        document.DocumentNumber = dto.DocumentNumber;
        document.CategoryId = dto.CategoryId ?? document.CategoryId;
        document.IssueDate = dto.IssueDate;
        document.ExpiryDate = dto.ExpiryDate;
        if (dto.Status != null) document.Status = dto.Status;
        
        document.UpdatedByUserId = userId;
        document.UpdatedAt = _dateTime.UtcNow;

        if (dto.RemoveFile)
        {
            if (!string.IsNullOrEmpty(document.FilePath))
                await _fileStorageService.DeleteAsync(document.FilePath);
            document.FilePath = null;
            document.FileName = null;
            document.FileSize = null;
            document.FileType = null;
        }
        else if (dto.File != null)
        {
            if (!string.IsNullOrEmpty(document.FilePath))
                await _fileStorageService.DeleteAsync(document.FilePath);

            var fileResult = await _fileStorageService.UploadAsync(dto.File, "documents");
            document.FilePath = fileResult.FilePath;
            document.FileName = fileResult.OriginalFileName;
            document.FileSize = fileResult.FileSize;
            document.FileType = fileResult.ContentType;
        }

        if (dto.Fields != null)
        {
            foreach (var updateField in dto.Fields)
            {
                if (updateField.Id.HasValue)
                {
                    var existingField = document.Fields.FirstOrDefault(f => f.Id == updateField.Id.Value);
                    if (existingField != null)
                    {
                        if (updateField.IsDeleted)
                            document.Fields.Remove(existingField);
                        else
                        {
                            existingField.FieldName = updateField.FieldName;
                            existingField.FieldValue = updateField.FieldValue;
                            existingField.FieldType = updateField.FieldType;
                            existingField.SortOrder = updateField.SortOrder;
                            existingField.UpdatedAt = _dateTime.UtcNow;
                        }
                    }
                }
                else if (!updateField.IsDeleted)
                {
                    document.Fields.Add(new DocumentField
                    {
                        Id = Guid.NewGuid(),
                        DocumentId = document.Id,
                        FieldName = updateField.FieldName,
                        FieldValue = updateField.FieldValue,
                        FieldType = updateField.FieldType,
                        SortOrder = updateField.SortOrder,
                        CreatedAt = _dateTime.UtcNow
                    });
                }
            }
        }

        _unitOfWork.Documents.Update(document);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(document.Id);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var document = await _unitOfWork.Documents.GetEntityByIdAsync(id);
        if (document == null)
            return false;

        if (!string.IsNullOrEmpty(document.FilePath))
            await _fileStorageService.DeleteAsync(document.FilePath);

        await _unitOfWork.Documents.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<(byte[] fileBytes, string fileName, string contentType)?> DownloadFileAsync(Guid id)
    {
        var document = await _unitOfWork.Documents.GetEntityByIdAsync(id);
        if (document == null || string.IsNullOrEmpty(document.FilePath))
            return null;

        var fileBytes = await _fileStorageService.DownloadAsync(document.FilePath);
        if (fileBytes == null)
            return null;

        var contentType = document.FileType ?? "application/octet-stream";
        return (fileBytes, document.FileName ?? "document", contentType);
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        return await _unitOfWork.Documents.GetDashboardStatsAsync();
    }
}

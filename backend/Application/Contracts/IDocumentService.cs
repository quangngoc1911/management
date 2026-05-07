using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Documents.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IDocumentService
{
    Task<PaginatedResultDto<DocumentListDto>> GetAllAsync(DocumentQueryDto query);
    Task<DocumentDto?> GetByIdAsync(Guid id);
    Task<DocumentDto> CreateAsync(CreateDocumentDto dto, Guid userId);
    Task<DocumentDto?> UpdateAsync(UpdateDocumentDto dto, Guid userId);
    Task<bool> DeleteAsync(Guid id);
    Task<(byte[] fileBytes, string fileName, string contentType)?> DownloadFileAsync(Guid id);
    Task<DashboardStatsDto> GetDashboardStatsAsync();
}
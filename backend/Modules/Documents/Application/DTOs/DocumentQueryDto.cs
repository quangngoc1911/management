namespace ManagementSystem.Modules.Documents.Application.DTOs;

public class DocumentQueryDto
{
    public string? Search { get; set; }
    public string? CategoryId { get; set; }
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; }
    public bool IsDescending { get; set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

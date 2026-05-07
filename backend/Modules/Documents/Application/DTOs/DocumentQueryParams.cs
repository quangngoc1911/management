namespace ManagementSystem.Modules.Documents.Application.DTOs;

/// <summary>
/// Query parameters for document list
/// </summary>
public class DocumentQueryParams
{
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public bool IncludeChildCategories { get; set; } = true;
    public string? FileType { get; set; }
    public string? TagSlug { get; set; }
    public string SortBy { get; set; } = "createdAt";
    public string SortDir { get; set; } = "desc";
    public int Page { get; set; } = 1;
    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

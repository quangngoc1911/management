namespace ManagementSystem.Application.DTOs.Requests;

public class BaseFilterRequest : PaginationRequest
{
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool IsDescending { get; set; }
}
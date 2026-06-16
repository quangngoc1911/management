namespace ManagementSystem.Modules.SystemAdmin.Application.DTOs;

public class SystemConfigQueryParams
{
    public string? Search { get; set; }
    public bool? IsPublic { get; set; }
    public string SortBy { get; set; } = "key";
    public bool IsDescending { get; set; } = false;
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Events.Application.DTOs;

public class FamilyEventQueryParams
{
    public string? Search { get; set; }
    public string? EventType { get; set; }
    public EventStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string SortBy { get; set; } = "startat";
    public bool IsDescending { get; set; } = true;
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

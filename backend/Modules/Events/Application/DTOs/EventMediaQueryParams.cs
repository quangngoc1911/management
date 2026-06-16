namespace ManagementSystem.Modules.Events.Application.DTOs;

public class EventMediaQueryParams
{
    public Guid? EventId { get; set; }
    public string SortBy { get; set; } = "sortorder";
    public bool IsDescending { get; set; } = false;
    public int Page { get; set; } = 1;

    private int _pageSize = 50;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 200 ? 200 : value < 1 ? 1 : value;
    }
}

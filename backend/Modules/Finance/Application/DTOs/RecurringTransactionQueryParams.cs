namespace ManagementSystem.Modules.Finance.Application.DTOs;

public class RecurringTransactionQueryParams
{
    public string? Search { get; set; }
    public Guid? AccountId { get; set; }
    public string? Type { get; set; }
    public string? Frequency { get; set; }
    public bool? IsActive { get; set; }
    public string SortBy { get; set; } = "nextduedate";
    public bool IsDescending { get; set; } = false;
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

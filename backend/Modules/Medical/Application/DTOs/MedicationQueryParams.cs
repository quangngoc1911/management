namespace ManagementSystem.Modules.Medical.Application.DTOs;

public class MedicationQueryParams
{
    public string? Search { get; set; }
    public Guid? MemberId { get; set; }
    public bool? IsActive { get; set; }
    public string SortBy { get; set; } = "name";
    public bool IsDescending { get; set; } = false;
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

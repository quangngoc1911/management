namespace ManagementSystem.Modules.Medical.Application.DTOs;

public class MedicalRecordQueryParams
{
    public string? Search { get; set; }
    public Guid? MemberId { get; set; }
    public string? RecordType { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public string SortBy { get; set; } = "recorddate";
    public bool IsDescending { get; set; } = true;
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Education.Application.DTOs;

public class StudyScheduleQueryParams
{
    public string? Search { get; set; }
    public Guid? MemberId { get; set; }
    public StudyScheduleStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string SortBy { get; set; } = "starttime";
    public bool IsDescending { get; set; } = true;
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

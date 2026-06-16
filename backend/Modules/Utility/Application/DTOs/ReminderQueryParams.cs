using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Utility.Application.DTOs;

public class ReminderQueryParams
{
    /// <summary>Set by the controller to the authenticated user (reminders are personal).</summary>
    public Guid? UserId { get; set; }
    public string? Search { get; set; }
    public ReminderStatus? Status { get; set; }
    public string SortBy { get; set; } = "remindat";
    public bool IsDescending { get; set; } = false;
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

namespace ManagementSystem.Modules.SystemAdmin.Application.DTOs;

public class NotificationQueryParams
{
    /// <summary>Set by the controller to the authenticated user (notifications are personal).</summary>
    public Guid? UserId { get; set; }
    public bool? IsRead { get; set; }
    public string? Type { get; set; }
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

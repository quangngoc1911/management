namespace ManagementSystem.Modules.Utility.Application.DTOs;

public class BookmarkQueryParams
{
    /// <summary>Set by the controller to the authenticated user (bookmarks are personal).</summary>
    public Guid? UserId { get; set; }
    public string? EntityType { get; set; }
    public bool IsDescending { get; set; } = true;
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

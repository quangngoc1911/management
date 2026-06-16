namespace ManagementSystem.Modules.SystemAdmin.Application.DTOs;

public class BackupLogQueryParams
{
    public string? BackupType { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

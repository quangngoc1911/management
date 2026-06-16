using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.SystemAdmin.Application.DTOs;

public class AuditLogQueryParams
{
    public Guid? UserId { get; set; }
    public AuditAction? Action { get; set; }
    public string? EntityType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}

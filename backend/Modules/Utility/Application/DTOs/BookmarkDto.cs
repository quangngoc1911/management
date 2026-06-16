namespace ManagementSystem.Modules.Utility.Application.DTOs;

public class BookmarkDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string? Note { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

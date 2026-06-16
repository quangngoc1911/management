namespace ManagementSystem.Modules.Utility.Application.DTOs;

public class ViewHistoryDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public DateTime ViewedAt { get; set; }
    public int? DurationSeconds { get; set; }
    public DateTime? CreatedAt { get; set; }
}

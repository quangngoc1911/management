namespace ManagementSystem.Modules.Utility.Application.DTOs;

public class CreateViewHistoryDto
{
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public DateTime ViewedAt { get; set; }
    public int? DurationSeconds { get; set; }

    /// <summary>Set by the controller from the authenticated user.</summary>
    public Guid UserId { get; set; }
}

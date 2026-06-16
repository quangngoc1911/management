namespace ManagementSystem.Modules.Utility.Application.DTOs;

public class CreateBookmarkDto
{
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string? Note { get; set; }

    /// <summary>Set by the controller from the authenticated user.</summary>
    public Guid UserId { get; set; }
}

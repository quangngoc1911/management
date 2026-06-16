namespace ManagementSystem.Modules.SystemAdmin.Application.DTOs;

public class CreateNotificationDto
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public string Type { get; set; } = "info";
    public string Channel { get; set; } = "in-app";
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
}

namespace ManagementSystem.Modules.Events.Application.DTOs;

public class CreateEventMediaDto
{
    public Guid EventId { get; set; }
    public Guid FileId { get; set; }
    public string? Caption { get; set; }
    public short? SortOrder { get; set; }

    /// <summary>Set by the controller from the authenticated user.</summary>
    public Guid? UploadedByUserId { get; set; }
}

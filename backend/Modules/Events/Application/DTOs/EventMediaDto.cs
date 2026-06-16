namespace ManagementSystem.Modules.Events.Application.DTOs;

public class EventMediaDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid FileId { get; set; }
    public string? Caption { get; set; }
    public short? SortOrder { get; set; }
    public Guid? UploadedByUserId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

namespace MyAPI.Models;

public class ViewHistory : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int DocumentId { get; set; }
    public Document Document { get; set; } = null!;

    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    public int? DurationSec { get; set; }
}
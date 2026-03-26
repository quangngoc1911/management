namespace MyAPI.Models;

public class Bookmark
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int DocumentId { get; set; }
    public Document Document { get; set; } = null!;

    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
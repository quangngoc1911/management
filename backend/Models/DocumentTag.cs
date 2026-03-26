namespace MyAPI.Models;

public class DocumentTag : BaseEntity
{
    public int DocumentId { get; set; }
    public Document Document { get; set; } = null!;

    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
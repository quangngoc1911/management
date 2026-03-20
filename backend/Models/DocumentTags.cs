namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class DocumentTags
{
    public int DocumentId { get; set; }
    [StringLength(256)]
    public string? TagId { get; set; }
    [StringLength(256)]
    public string? AddedAt { get; set; }
    public Documents Document { get; set; }

    public int TagsId { get; set; } // FK
    public Tags Tag { get; set; }
}
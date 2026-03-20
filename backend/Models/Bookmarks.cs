namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class Bookmarks
{
    public int BookmarksId { get; set; }
    public int UserId { get; set; }
    [StringLength(256)]
    public string? DocumentId { get; set; }
    [StringLength(256)]
    public string? Note { get; set; }
    [StringLength(256)]
    public string? CreatedAt { get; set; }

    public User User { get; set; }

    public int DocumentsId { get; set; } // FK
    public Documents Document { get; set; }

}

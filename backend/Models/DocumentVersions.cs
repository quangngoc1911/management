namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class DocumentVersions
{
    public int DocumentVersionsId { get; set; }
    [StringLength(256)]
    public string? DocumentId { get; set; }
    [StringLength(256)]
    public string? VersionNumber { get; set; }
    public int Title { get; set; }
    [StringLength(256)]
    public string? Content { get; set; }
    [StringLength(256)]
    public string? ChangeSummary { get; set; }
    public int EditedByUserId { get; set; }
    [StringLength(256)]
    public string? CreatedAt { get; set; }

    public int DocumentsId { get; set; } // FK
    public Documents Document { get; set; }

    public string Contents { get; set; }

}
namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class ViewHistories
{
    public int ViewHistoriesId { get; set; }
    [StringLength(256)]
    public string? UserId { get; set; }
    [StringLength(256)]
    public string? DocumentId { get; set; }
    [StringLength(256)]
    public string? ViewedAt { get; set; }
    [StringLength(256)]
    public string? DurationSec { get; set; }

    public User User { get; set; }

    public int DocumentsId { get; set; }
    public Documents Document { get; set; }

    public DateTime ViewedAts { get; set; }
}
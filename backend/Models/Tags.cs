namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class Tags
{
    public int TagsId { get; set; }
    [StringLength(256)]
    public string? TagsName { get; set; }
    [StringLength(256)]
    public string? Slug { get; set; }
    [StringLength(256)]
    public string? Color { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public List<DocumentTags> DocumentTags { get; set; }


}
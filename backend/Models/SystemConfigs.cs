namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class SystemConfigs
{
    public int SystemConfigsId { get; set; }
    [StringLength(256)]
    public string? Key { get; set; }
    [StringLength(256)]
    public string? Value { get; set; }
    [StringLength(256)]
    public string? Description { get; set; }
    [StringLength(256)]
    public string? IsPublic { get; set; }
    [StringLength(256)]
    public string? UpdatedAt { get; set; }
}
using System;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Modules.Auth.Domain.Entities;

public class Setting : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystem { get; set; } = false;
}
namespace ManagementSystem.Modules.Documents.Application.DTOs;

public class CreateTagDto
{
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Color { get; set; }
}

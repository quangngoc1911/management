public class CreateMenuDto
{
    public string Name { get; set; } = null!;
    // public string? Slug { get; set; }
    public string? Path { get; set; }
    public string? Icon { get; set; }
    public int Order { get; set; } = 0;
    public bool IsVisible { get; set; } = true;
    // public string? RequiredRole { get; set; }
    public int? ParentId { get; set; }
}
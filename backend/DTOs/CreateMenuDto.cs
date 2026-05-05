public class CreateMenuDto
{
    public string Name { get; set; } = null!;
    // public string? Slug { get; set; }
    public string? Path { get; set; }
    public string? Icon { get; set; }
    public int Order { get; set; } = 0;
    public bool IsVisible { get; set; } = true;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid? ParentId { get; set; }
}
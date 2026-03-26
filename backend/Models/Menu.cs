using MyAPI.Models;

public class Menu : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Slug { get; set; } = null!;
    public string? Path { get; set; }       // URL path, e.g. "/documents"
    public string? Icon { get; set; }       // e.g. "fa-home"
    public int Order { get; set; }          // thứ tự hiển thị
    public bool IsVisible { get; set; } = true;

    public int? ParentId { get; set; }
    public Menu? Parent { get; set; }
    public ICollection<Menu> Children { get; set; } = new List<Menu>();
}
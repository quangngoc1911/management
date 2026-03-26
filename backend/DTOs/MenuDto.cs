public class MenuDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Path { get; set; }
    public string? Icon { get; set; }
    public int Order { get; set; }
    public List<MenuDto> Children { get; set; } = new();
}
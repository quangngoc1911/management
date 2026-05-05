using System;
using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.DTOs.Menu;

public class CreateMenuDto
{
    [Required(ErrorMessage = "Tên menu là bắt buộc")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên menu phải có từ 3 đến 100 ký tự")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Đường dẫn không được vượt quá 200 ký tự")]
    public string? Path { get; set; }

    [StringLength(100, ErrorMessage = "Icon không được vượt quá 100 ký tự")]
    public string? Icon { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public Guid? ParentId { get; set; }
}

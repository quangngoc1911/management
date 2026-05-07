using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Modules.Users.Application.DTOs;

public class CreateUserDto
{
    [Required(ErrorMessage = "Tên người dùng không được để trống")]
    [StringLength(100, ErrorMessage = "Tên người dùng không được vượt quá 100 ký tự")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự và không quá 100 ký tự")]
    public string Password { get; set; } = string.Empty;

    public string? Role { get; set; } = "Viewer"; // Admin, Editor, Viewer
    public string? AvatarUrl { get; set; }
    public string? Department { get; set; }
    public bool IsActive { get; set; } = true;
}
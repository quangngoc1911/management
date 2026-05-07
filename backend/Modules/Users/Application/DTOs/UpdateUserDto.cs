using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Modules.Users.Application.DTOs;

public class UpdateUserDto
{
    [StringLength(100, ErrorMessage = "Tên người dùng không được vượt quá 100 ký tự")]
    public string? Name { get; set; }

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự")]
    public string? Email { get; set; }

    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự và không quá 100 ký tự")]
    public string? Password { get; set; }

    public string? Role { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Department { get; set; }
    public bool? IsActive { get; set; }
}
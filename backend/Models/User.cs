namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class User
{
    public int UserId { get; set; }
    [StringLength(150)]
    public string? Name { get; set; }
    [StringLength(256)]
    public string? Email { get; set; }
    [MaxLength(500)]
    public string? PasswordHash { get; set; }
    [MaxLength(20)]
    public string? Role { get; set; }
    [MaxLength(50)]
    public string? AvatarUrl { get; set; }

    public string? LastLoginAt { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public string? IsDeleted { get; set; }

    public List<Documents> Documents { get; set; }
    public List<Bookmarks> Bookmarks { get; set; }
    public List<ViewHistories> ViewHistories { get; set; }
    public List<RefreshTokens> RefreshTokens { get; set; }


}
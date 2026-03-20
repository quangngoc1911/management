namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class RefreshTokens
{
    public int RefreshTokensId { get; set; }
    [StringLength(256)]
    public string? UserId { get; set; }
    [StringLength(256)]
    public string? TokenHash { get; set; }
    [StringLength(256)]
    public string? DeviceInfo { get; set; }
    [StringLength(256)]
    public string? ExpiresAt { get; set; }
    [StringLength(256)]
    public string? IsRevoked { get; set; }
    [StringLength(256)]
    public string? CreatedAt { get; set; }

    public User User { get; set; }

    public string Token { get; set; }
}
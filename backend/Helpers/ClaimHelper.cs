using System.Security.Claims;


namespace MyAPI.Helpers;
// ============================================================
// CLAIM HELPERS — Lấy thông tin từ JWT Token
// ============================================================
public static class ClaimHelper
{
    /// <summary>
    /// Lấy UserId từ JWT token đang đăng nhập.
    /// Dùng trong Controller: var userId = User.GetUserId();
    /// </summary>
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(value, out var id) ? id : 0;
    }

    /// <summary>
    /// Lấy Email từ JWT token.
    /// </summary>
    public static string GetEmail(this ClaimsPrincipal user)
        => user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

    /// <summary>
    /// Lấy Role từ JWT token.
    /// </summary>
    public static string GetRole(this ClaimsPrincipal user)
        => user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

    /// <summary>
    /// Kiểm tra user có role Admin không.
    /// </summary>
    public static bool IsAdmin(this ClaimsPrincipal user)
        => user.GetRole() == "Admin";

    /// <summary>
    /// Kiểm tra user đã đăng nhập chưa.
    /// </summary>
    public static bool IsLoggedIn(this ClaimsPrincipal user)
        => user.GetUserId() > 0;
}

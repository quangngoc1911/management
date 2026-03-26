using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MyAPI.Helpers;
// ============================================================
// STRING HELPERS — Xử lý chuỗi
// ============================================================
public static class StringHelper
{
    /// <summary>
    /// Chuyển chuỗi thành slug thân thiện với URL.
    /// Ví dụ: "Tài liệu Kỹ Thuật!" → "tai-lieu-ky-thuat"
    /// </summary>
    public static string ToSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        // Bỏ dấu tiếng Việt
        var normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);

        return sb.ToString()
            .Normalize(NormalizationForm.FormC)
            .ToLower()
            .Replace("đ", "d")
            .Trim()
            .Replace(" ", "-")
            .Replace("_", "-")
            // Xóa ký tự đặc biệt (giữ chữ, số, dấu -)
            .Replace(Regex.Replace(sb.ToString(), @"[^a-z0-9\-]", ""), "")
            .Pipe(s => Regex.Replace(s, @"[^a-z0-9\-]", ""))
            // Xóa dấu -- liên tiếp
            .Pipe(s => Regex.Replace(s, @"-+", "-"))
            .Trim('-');
    }

    /// <summary>
    /// Cắt chuỗi nếu quá dài, thêm "..." ở cuối.
    /// Ví dụ: Truncate("Hello World", 5) → "Hello..."
    /// </summary>
    public static string Truncate(string input, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
            return input;

        return input[..maxLength] + suffix;
    }

    /// <summary>
    /// Chuẩn hóa email: trim + lowercase.
    /// Ví dụ: "  User@Gmail.COM  " → "user@gmail.com"
    /// </summary>
    public static string NormalizeEmail(string email)
        => email.Trim().ToLower();

    /// <summary>
    /// Kiểm tra chuỗi có phải email hợp lệ không.
    /// </summary>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    /// <summary>
    /// Xóa khoảng trắng thừa giữa các từ.
    /// Ví dụ: "Hello   World" → "Hello World"
    /// </summary>
    public static string NormalizeWhitespace(string input)
        => Regex.Replace(input.Trim(), @"\s+", " ");

    // Hàm nội bộ hỗ trợ pipe
    private static T Pipe<T>(this T value, Func<T, T> func) => func(value);
}

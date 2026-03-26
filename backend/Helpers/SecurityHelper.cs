using System.Security.Cryptography;
using System.Text;

namespace MyAPI.Helpers;
// ============================================================
// SECURITY HELPERS — Bảo mật
// ============================================================
public static class SecurityHelper
{
    /// <summary>
    /// Tạo chuỗi random an toàn dùng cho token, OTP...
    /// Ví dụ: GenerateRandomString(32) → "aB3xK9..."
    /// </summary>
    public static string GenerateRandomString(int length = 32)
    {
        var bytes = RandomNumberGenerator.GetBytes(length);
        return Convert.ToBase64String(bytes)[..length];
    }

    /// <summary>
    /// Tạo OTP số có độ dài tùy chỉnh.
    /// Ví dụ: GenerateOtp(6) → "482931"
    /// </summary>
    public static string GenerateOtp(int length = 6)
    {
        var bytes = RandomNumberGenerator.GetBytes(4);
        var number = BitConverter.ToUInt32(bytes, 0);
        return (number % (uint)Math.Pow(10, length))
            .ToString()
            .PadLeft(length, '0');
    }

    /// <summary>
    /// Hash chuỗi bằng SHA256.
    /// Dùng để hash token trước khi lưu DB.
    /// </summary>
    public static string HashSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLower();
    }

    /// <summary>
    /// So sánh 2 chuỗi an toàn (tránh timing attack).
    /// Dùng khi so sánh token, không dùng == trực tiếp.
    /// </summary>
    public static bool SecureCompare(string a, string b)
        => CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(a),
            Encoding.UTF8.GetBytes(b));
}

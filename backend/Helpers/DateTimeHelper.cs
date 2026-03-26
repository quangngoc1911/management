namespace MyAPI.Helpers;

// ============================================================
// DATETIME HELPERS — Xử lý ngày giờ
// ============================================================
public static class DateTimeHelper
{
    /// <summary>
    /// Lấy thời gian hiện tại theo UTC.
    /// Luôn dùng UTC trong DB, chỉ convert sang local khi hiển thị.
    /// </summary>
    public static DateTime UtcNow => DateTime.UtcNow;

    /// <summary>
    /// Chuyển UTC sang múi giờ Việt Nam (UTC+7).
    /// </summary>
    public static DateTime ToVietnamTime(DateTime utcTime)
    {
        var vnZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, vnZone);
    }

    /// <summary>
    /// Hiển thị thời gian tương đối.
    /// Ví dụ: "2 giờ trước", "3 ngày trước"
    /// </summary>
    public static string ToRelativeTime(DateTime utcTime)
    {
        var diff = DateTime.UtcNow - utcTime;

        return diff.TotalSeconds switch
        {
            < 60 => "Vừa xong",
            < 3600 => $"{(int)diff.TotalMinutes} phút trước",
            < 86400 => $"{(int)diff.TotalHours} giờ trước",
            < 2592000 => $"{(int)diff.TotalDays} ngày trước",
            < 31536000 => $"{(int)(diff.TotalDays / 30)} tháng trước",
            _ => $"{(int)(diff.TotalDays / 365)} năm trước"
        };
    }

    /// <summary>
    /// Format ngày theo kiểu Việt Nam.
    /// Ví dụ: 25/03/2026 14:30
    /// </summary>
    public static string ToVnFormat(DateTime dt, bool includeTime = true)
        => includeTime
            ? dt.ToString("dd/MM/yyyy HH:mm")
            : dt.ToString("dd/MM/yyyy");

    /// <summary>
    /// Kiểm tra ngày có nằm trong khoảng không.
    /// </summary>
    public static bool IsBetween(DateTime date, DateTime from, DateTime to)
        => date >= from && date <= to;

    /// <summary>
    /// Lấy ngày đầu tiên của tháng.
    /// </summary>
    public static DateTime StartOfMonth(DateTime date)
        => new(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Lấy ngày cuối cùng của tháng.
    /// </summary>
    public static DateTime EndOfMonth(DateTime date)
        => new(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month),
               23, 59, 59, DateTimeKind.Utc);
}

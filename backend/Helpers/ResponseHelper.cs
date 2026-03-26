namespace MyAPI.Helpers;

// ============================================================
// RESPONSE HELPERS — Chuẩn hóa response API
// ============================================================

/// <summary>
/// Chuẩn hóa format response cho toàn bộ API.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    /// <summary>Trả về response thành công kèm data.</summary>
    public static ApiResponse<T> Ok(T data, string? message = null)
        => new() { Success = true, Data = data, Message = message };

    /// <summary>Trả về response lỗi kèm message.</summary>
    public static ApiResponse<T> Fail(string message)
        => new() { Success = false, Message = message };
}

// Phiên bản không có data (chỉ trả message)
public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Ok(string message)
        => new() { Success = true, Message = message };

    public new static ApiResponse Fail(string message)
        => new() { Success = false, Message = message };
}
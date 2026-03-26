
namespace MyAPI.Helpers;
// ============================================================
// PAGINATION HELPERS — Phân trang
// ============================================================

/// <summary>
/// Request phân trang — dùng làm tham số query.
/// Ví dụ: GET /api/documents?page=1&pageSize=10
/// </summary>
public class PageRequest
{
    private int _page = 1;
    private int _pageSize = 10;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value is < 1 or > 100 ? 10 : value;
    }

    /// <summary>Số bản ghi bỏ qua — dùng trong LINQ Skip()</summary>
    public int Skip => (Page - 1) * PageSize;
}

/// <summary>
/// Response phân trang — bao gồm data + metadata.
/// </summary>
public class PagedResult<T>
{
    public List<T> Data { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNext => Page < TotalPages;
    public bool HasPrev => Page > 1;

    /// <summary>
    /// Tạo PagedResult từ list và request.
    /// Ví dụ: PagedResult<UserDto>.From(users, request, total)
    /// </summary>
    public static PagedResult<T> From(List<T> data, PageRequest request, int totalCount)
        => new()
        {
            Data = data,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
}

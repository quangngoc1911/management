namespace ManagementSystem.Domain.Enums;

public enum EntityAction
{
    /// <summary>
    /// Đang chờ xử lý hoặc bản nháp
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Đang hoạt động bình thường
    /// </summary>
    Active = 1,

    /// <summary>
    /// Đang tạm ngưng/vô hiệu hóa
    /// </summary>
    Inactive = 2,

    /// <summary>
    /// Đã lưu trữ (không xuất hiện trong danh sách thường lệ nhưng vẫn tồn tại)
    /// </summary>
    Archived = 3,

    /// <summary>
    /// Trạng thái đánh dấu đã xóa (kết hợp với IsDeleted)
    /// </summary>
    Deleted = 4
}
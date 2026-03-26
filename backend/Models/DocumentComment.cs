// =========================
// Models/DocumentComment.cs (Phase 2 - optional)
// Tài liệu chỉ nhắc Comments ở tổng quan, chưa có bảng chi tiết.
// =========================
namespace MyAPI.Models;

public class DocumentComment : BaseEntity
{
    public int DocumentId { get; set; }
    public Document Document { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string Content { get; set; } = string.Empty;
}
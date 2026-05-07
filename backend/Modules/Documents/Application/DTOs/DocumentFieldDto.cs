namespace ManagementSystem.Modules.Documents.Application.DTOs;

public class DocumentFieldDto
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public string FieldValue { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

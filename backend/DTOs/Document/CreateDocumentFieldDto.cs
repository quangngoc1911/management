using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.DTOs.Document;

public class CreateDocumentFieldDto
{
    [Required]
    [StringLength(200)]
    public string FieldName { get; set; } = string.Empty;

    [Required]
    public string FieldValue { get; set; } = string.Empty;

    [StringLength(50)]
    public string FieldType { get; set; } = "Text";

    public int SortOrder { get; set; }
}

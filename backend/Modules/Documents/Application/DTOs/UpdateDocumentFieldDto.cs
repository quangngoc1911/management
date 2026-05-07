using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Modules.Documents.Application.DTOs;

public class UpdateDocumentFieldDto
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(200)]
    public string FieldName { get; set; } = string.Empty;

    [Required]
    public string FieldValue { get; set; } = string.Empty;

    [StringLength(50)]
    public string FieldType { get; set; } = "Text";

    public int SortOrder { get; set; }

    public bool IsDeleted { get; set; } = false;
}

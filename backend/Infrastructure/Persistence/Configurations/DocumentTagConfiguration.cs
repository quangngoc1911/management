using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class DocumentTagConfiguration : IEntityTypeConfiguration<DocumentTag>
{
    public void Configure(EntityTypeBuilder<DocumentTag> builder)
    {
        builder.HasKey(dt => dt.Id);

        builder.HasIndex(dt => new { dt.DocumentId, dt.TagId })
            .IsUnique();
    }
}


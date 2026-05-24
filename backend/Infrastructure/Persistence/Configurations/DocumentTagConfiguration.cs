using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class DocumentTagConfiguration : IEntityTypeConfiguration<DocumentTag>
{
    public void Configure(EntityTypeBuilder<DocumentTag> builder)
    {
        builder.HasKey(dt => dt.Id);
        builder.Property(dt => dt.Id).HasColumnOrder(0);

        builder.HasIndex(dt => new { dt.DocumentId, dt.TagId }).IsUnique();
        builder.Property(dt => dt.DocumentId).IsRequired().HasColumnOrder(1);
        builder.Property(dt => dt.TagId).IsRequired().HasColumnOrder(2);

        builder.Property(dt => dt.AddedAt).IsRequired().HasColumnOrder(3);
    }
}

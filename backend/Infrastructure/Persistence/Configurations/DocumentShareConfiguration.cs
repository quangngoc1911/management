using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class DocumentShareConfiguration : IEntityTypeConfiguration<DocumentShare>
{
    public void Configure(EntityTypeBuilder<DocumentShare> builder)
    {
        builder.HasKey(ds => ds.Id);

        builder.Property(ds => ds.PermissionLevel)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(ds => new { ds.DocumentId, ds.SharedWithUserId })
            .IsUnique();
    }
}


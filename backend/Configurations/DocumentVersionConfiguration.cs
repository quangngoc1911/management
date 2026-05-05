using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Entities;

namespace ManagementSystem.Data.Configurations;

public class DocumentVersionConfiguration : IEntityTypeConfiguration<DocumentVersion>
{
    public void Configure(EntityTypeBuilder<DocumentVersion> builder)
    {
        builder.HasKey(dv => dv.Id);

        builder.Property(dv => dv.VersionNumber)
            .IsRequired();

        builder.Property(dv => dv.Title)
            .HasMaxLength(200);

        builder.Property(dv => dv.FilePath)
            .HasMaxLength(500);

        builder.Property(dv => dv.FileName)
            .HasMaxLength(255);

        builder.Property(dv => dv.ChangeLog)
            .HasMaxLength(1000);

        builder.HasIndex(dv => new { dv.DocumentId, dv.VersionNumber })
            .IsUnique();
    }
}
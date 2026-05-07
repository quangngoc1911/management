using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class DocumentFileConfiguration : IEntityTypeConfiguration<DocumentFile>
{
    public void Configure(EntityTypeBuilder<DocumentFile> builder)
    {
        builder.HasKey(df => df.Id);

        builder.Property(df => df.OriginalName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(df => df.StoredName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(df => df.StoragePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(df => df.PublicUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(df => df.FileType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(df => df.MimeType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(df => df.StorageProvider)
            .IsRequired()
            .HasMaxLength(50);
    }
}

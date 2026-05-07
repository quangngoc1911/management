using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.FileType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ThumbnailUrl)
            .HasMaxLength(500);
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class DocumentFileConfiguration : IEntityTypeConfiguration<DocumentFile>
{
    public void Configure(EntityTypeBuilder<DocumentFile> builder)
    {
        builder.ToTable("files");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnOrder(0);

        builder.Property(f => f.OriginalName)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(true)
            .HasColumnOrder(1);

        builder.Property(f => f.StoredName)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnOrder(2);

        builder.Property(f => f.StoragePath)
            .IsRequired()
            .HasMaxLength(1000)
            .HasColumnOrder(3);

        builder.Property(f => f.PublicUrl)
            .IsRequired()
            .HasMaxLength(1000)
            .HasColumnOrder(4);

        builder.Property(f => f.FileType)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnOrder(5);

        builder.Property(f => f.MimeType)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(6);

        builder.Property(f => f.SizeBytes).IsRequired().HasColumnOrder(7);

        builder.Property(f => f.UploadedByUserId).IsRequired().HasColumnOrder(8);

        builder.Property(f => f.StorageProvider)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("local")
            .HasColumnOrder(9);

        builder.Property(f => f.CreatedAt).IsRequired().HasColumnOrder(10);
        builder.Property(f => f.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(11);

        builder.HasOne(f => f.UploadedByUser)
            .WithMany()
            .HasForeignKey(f => f.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

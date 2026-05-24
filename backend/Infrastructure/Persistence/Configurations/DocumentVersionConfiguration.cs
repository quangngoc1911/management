using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class DocumentVersionConfiguration : IEntityTypeConfiguration<DocumentVersion>
{
    public void Configure(EntityTypeBuilder<DocumentVersion> builder)
    {
        builder.HasKey(dv => dv.Id);
        builder.Property(dv => dv.Id).HasColumnOrder(0);

        builder.HasIndex(dv => new { dv.DocumentId, dv.VersionNumber }).IsUnique();
        builder.Property(dv => dv.DocumentId).IsRequired().HasColumnOrder(1);
        builder.Property(dv => dv.VersionNumber).IsRequired().HasColumnOrder(2);

        builder.Property(dv => dv.Title)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(true)
            .HasColumnOrder(3);

        builder.Property(dv => dv.Content)
            .HasColumnType("text")
            .HasColumnOrder(4);

        builder.Property(dv => dv.ChangeSummary)
            .HasMaxLength(500)
            .HasColumnOrder(5);

        builder.Property(dv => dv.EditedByUserId).IsRequired().HasColumnOrder(6);

        builder.Property(dv => dv.CreatedAt).IsRequired().HasColumnOrder(7);

        builder.HasOne(dv => dv.EditedByUser)
            .WithMany(u => u.EditedVersions)
            .HasForeignKey(dv => dv.EditedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

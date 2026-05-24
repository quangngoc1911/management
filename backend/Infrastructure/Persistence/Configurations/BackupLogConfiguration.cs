using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class BackupLogConfiguration : IEntityTypeConfiguration<BackupLog>
{
    public void Configure(EntityTypeBuilder<BackupLog> builder)
    {
        builder.HasKey(bl => bl.Id);
        builder.Property(bl => bl.Id).HasColumnOrder(0);

        builder.Property(bl => bl.BackupType).IsRequired().HasMaxLength(50).HasColumnOrder(1);

        builder.HasIndex(bl => bl.Status);
        builder.Property(bl => bl.Status).IsRequired().HasMaxLength(50).HasColumnOrder(2);

        builder.Property(bl => bl.FilePath).HasColumnType("text").HasColumnOrder(3);
        builder.Property(bl => bl.FileSize).HasColumnOrder(4);
        builder.Property(bl => bl.Checksum).HasMaxLength(64).HasColumnOrder(5);

        builder.Property(bl => bl.StartedAt).HasColumnOrder(6);
        builder.Property(bl => bl.CompletedAt).HasColumnOrder(7);

        builder.Property(bl => bl.ErrorMessage).HasColumnType("text").HasColumnOrder(8);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class SystemConfigConfiguration : IEntityTypeConfiguration<SystemConfig>
{
    public void Configure(EntityTypeBuilder<SystemConfig> builder)
    {
        builder.HasKey(sc => sc.Id);
        builder.Property(sc => sc.Id).HasColumnOrder(0);

        builder.HasIndex(sc => sc.Key).IsUnique();
        builder.Property(sc => sc.Key).IsRequired().HasMaxLength(255).HasColumnOrder(1);

        builder.Property(sc => sc.Value)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnOrder(2);

        builder.Property(sc => sc.Description).HasColumnType("text").HasColumnOrder(3);

        builder.Property(sc => sc.IsEncrypted).IsRequired().HasDefaultValue(false).HasColumnOrder(4);
        builder.Property(sc => sc.IsPublic).IsRequired().HasDefaultValue(false).HasColumnOrder(5);

        builder.Property(sc => sc.CreatedAt).IsRequired().HasColumnOrder(6);
    }
}

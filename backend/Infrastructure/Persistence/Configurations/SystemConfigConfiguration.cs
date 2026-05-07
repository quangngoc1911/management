using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class SystemConfigConfiguration : IEntityTypeConfiguration<SystemConfig>
{
    public void Configure(EntityTypeBuilder<SystemConfig> builder)
    {
        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sc => sc.Value)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(sc => sc.Description)
            .HasMaxLength(500);

        builder.Property(sc => sc.Group)
            .HasMaxLength(100);
    }
}

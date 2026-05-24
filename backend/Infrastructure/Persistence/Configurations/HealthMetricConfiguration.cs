using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class HealthMetricConfiguration : IEntityTypeConfiguration<HealthMetric>
{
    public void Configure(EntityTypeBuilder<HealthMetric> builder)
    {
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasColumnOrder(0);

        builder.HasIndex(h => h.MemberId);
        builder.Property(h => h.MemberId).IsRequired().HasColumnOrder(1);

        builder.HasIndex(h => h.MetricType);
        builder.Property(h => h.MetricType).IsRequired().HasMaxLength(100).HasColumnOrder(2);

        builder.Property(h => h.Value).IsRequired().HasPrecision(10, 3).HasColumnOrder(3);

        builder.Property(h => h.Unit).IsRequired().HasMaxLength(20).HasColumnOrder(4);

        builder.HasIndex(h => h.MeasuredAt);
        builder.Property(h => h.MeasuredAt).IsRequired().HasColumnOrder(5);

        builder.Property(h => h.Notes).HasColumnType("text").HasColumnOrder(6);

        builder.Property(h => h.CreatedAt).IsRequired().HasColumnOrder(7);

        builder.HasOne(h => h.Member).WithMany().HasForeignKey(h => h.MemberId).OnDelete(DeleteBehavior.Cascade);
    }
}

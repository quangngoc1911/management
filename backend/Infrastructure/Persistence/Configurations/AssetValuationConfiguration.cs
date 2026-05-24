using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Assets.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class AssetValuationConfiguration : IEntityTypeConfiguration<AssetValuation>
{
    public void Configure(EntityTypeBuilder<AssetValuation> builder)
    {
        builder.HasKey(av => av.Id);
        builder.Property(av => av.Id).HasColumnOrder(0);

        builder.HasIndex(av => av.AssetId);
        builder.Property(av => av.AssetId).IsRequired().HasColumnOrder(1);

        builder.Property(av => av.ValuationDate).IsRequired().HasColumnOrder(2);
        builder.Property(av => av.Value).IsRequired().HasPrecision(15, 2).HasColumnOrder(3);

        builder.Property(av => av.Currency).IsRequired().HasMaxLength(3).IsFixedLength().HasColumnOrder(4);

        builder.Property(av => av.ValuationMethod).HasMaxLength(100).HasColumnOrder(5);
        builder.Property(av => av.Notes).HasColumnType("text").HasColumnOrder(6);

        builder.Property(av => av.CreatedByUserId).HasColumnName("created_by").HasColumnOrder(7);
        builder.Property(av => av.CreatedAt).IsRequired().HasColumnOrder(8);

        builder.HasOne(av => av.Asset).WithMany(a => a.Valuations).HasForeignKey(av => av.AssetId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(av => av.CreatedByUser).WithMany().HasForeignKey(av => av.CreatedByUserId).OnDelete(DeleteBehavior.SetNull);
    }
}

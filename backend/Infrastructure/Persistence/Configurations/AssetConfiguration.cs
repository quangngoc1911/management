using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Assets.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnOrder(0);

        builder.Property(a => a.MemberId).HasColumnOrder(1);
        builder.Property(a => a.CategoryId).HasColumnOrder(2);

        builder.Property(a => a.Name).IsRequired().HasMaxLength(255).IsUnicode(true).HasColumnOrder(3);
        builder.Property(a => a.Description).HasColumnType("text").HasColumnOrder(4);

        builder.Property(a => a.AssetType).IsRequired().HasMaxLength(100).HasColumnOrder(5);

        builder.Property(a => a.PurchasePrice).HasPrecision(15, 2).HasColumnOrder(6);
        builder.Property(a => a.PurchaseDate).HasColumnOrder(7);

        builder.Property(a => a.Currency).HasMaxLength(3).IsFixedLength().HasColumnOrder(8);

        builder.Property(a => a.Location).HasColumnType("text").HasColumnOrder(9);
        builder.Property(a => a.SerialNumber).HasMaxLength(255).HasColumnOrder(10);

        builder.Property(a => a.Status).IsRequired().HasConversion<int>().HasColumnOrder(11);

        builder.Property(a => a.IsInsured).HasColumnOrder(12);
        builder.Property(a => a.InsuranceInfo).HasColumnType("jsonb").HasColumnOrder(13);
        builder.Property(a => a.Metadata).HasColumnType("jsonb").HasColumnOrder(14);

        builder.Property(a => a.CreatedByUserId).HasColumnName("created_by").HasColumnOrder(15);

        builder.Property(a => a.CreatedAt).IsRequired().HasColumnOrder(16);
        builder.Property(a => a.UpdatedAt).IsRequired().HasColumnOrder(17);
        builder.Property(a => a.DeletedAt).HasColumnOrder(18);

        builder.HasOne(a => a.Member).WithMany().HasForeignKey(a => a.MemberId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(a => a.Category).WithMany().HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(a => a.CreatedByUser).WithMany().HasForeignKey(a => a.CreatedByUserId).OnDelete(DeleteBehavior.SetNull);
    }
}

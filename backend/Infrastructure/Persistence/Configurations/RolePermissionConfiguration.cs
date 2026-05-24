using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(rp => rp.Id);
        builder.Property(rp => rp.Id).HasColumnOrder(0);

        builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();
        builder.HasIndex(rp => rp.RoleId);

        builder.Property(rp => rp.RoleId).IsRequired().HasColumnOrder(1);
        builder.Property(rp => rp.PermissionId).IsRequired().HasColumnOrder(2);
        builder.Property(rp => rp.GrantedBy).HasColumnOrder(3);
        builder.Property(rp => rp.RevokedAt).HasColumnOrder(4);

        builder.Property(rp => rp.CreatedAt).IsRequired().HasColumnOrder(5);

        builder.HasOne(rp => rp.Granter)
            .WithMany()
            .HasForeignKey(rp => rp.GrantedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

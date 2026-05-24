using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnOrder(0);

        builder.HasIndex(p => p.Name).IsUnique();
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnOrder(1);

        builder.HasIndex(p => p.Module);
        builder.Property(p => p.Module)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(p => p.Action)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnOrder(3);

        builder.Property(p => p.Description)
            .HasColumnType("text")
            .HasColumnOrder(4);

        builder.Property(p => p.IsSystem)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnOrder(5);

        // Permissions table has no audit columns per schema

        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

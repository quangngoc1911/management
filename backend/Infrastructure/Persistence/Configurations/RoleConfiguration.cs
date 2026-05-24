using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnOrder(0);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder.HasIndex(r => r.Slug).IsUnique();
        builder.Property(r => r.Slug)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(r => r.Description)
            .HasColumnType("text")
            .HasColumnOrder(3);

        builder.Property(r => r.Permissions)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasDefaultValueSql("'{}'::jsonb")
            .HasColumnOrder(4);

        builder.Property(r => r.IsSystem)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnOrder(5);

        // Audit + soft-delete
        builder.Property(r => r.CreatedAt).IsRequired().HasColumnOrder(6);
        builder.Property(r => r.UpdatedAt).IsRequired().HasColumnOrder(7);
        builder.Property(r => r.DeletedAt).HasColumnOrder(8);
        builder.Property(r => r.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(9);

        // Relationships
        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

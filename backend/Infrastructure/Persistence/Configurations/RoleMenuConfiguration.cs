using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class RoleMenuConfiguration : IEntityTypeConfiguration<RoleMenu>
{
    public void Configure(EntityTypeBuilder<RoleMenu> builder)
    {
        builder.HasKey(rm => rm.Id);

        builder.HasIndex(rm => new { rm.RoleId, rm.MenuId })
            .IsUnique();
    }
}


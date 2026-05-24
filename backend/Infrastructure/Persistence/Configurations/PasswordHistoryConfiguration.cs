using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class PasswordHistoryConfiguration : IEntityTypeConfiguration<PasswordHistory>
{
    public void Configure(EntityTypeBuilder<PasswordHistory> builder)
    {
        builder.HasKey(ph => ph.Id);
        builder.Property(ph => ph.Id).HasColumnOrder(0);

        builder.HasIndex(ph => ph.UserId);
        builder.Property(ph => ph.UserId).IsRequired().HasColumnOrder(1);

        builder.Property(ph => ph.PasswordHash)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnOrder(2);

        builder.HasIndex(ph => ph.ChangedAt);
        builder.Property(ph => ph.ChangedAt).IsRequired().HasColumnOrder(3);
    }
}

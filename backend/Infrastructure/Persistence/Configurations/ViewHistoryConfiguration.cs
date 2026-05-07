using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class ViewHistoryConfiguration : IEntityTypeConfiguration<ViewHistory>
{
    public void Configure(EntityTypeBuilder<ViewHistory> builder)
    {
        builder.HasKey(vh => vh.Id);

        builder.Property(vh => vh.ViewedAt)
            .IsRequired();

        builder.Property(vh => vh.Duration)
            .IsRequired();
    }
}

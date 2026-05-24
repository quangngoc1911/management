using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Events.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class EventMediaConfiguration : IEntityTypeConfiguration<EventMedia>
{
    public void Configure(EntityTypeBuilder<EventMedia> builder)
    {
        builder.HasKey(em => em.Id);
        builder.Property(em => em.Id).HasColumnOrder(0);

        builder.HasIndex(em => em.EventId);
        builder.Property(em => em.EventId).IsRequired().HasColumnOrder(1);

        builder.Property(em => em.FileId).IsRequired().HasColumnOrder(2);

        builder.Property(em => em.Caption).HasColumnType("text").HasColumnOrder(3);
        builder.Property(em => em.SortOrder).HasColumnOrder(4);
        builder.Property(em => em.UploadedByUserId).HasColumnName("uploaded_by").HasColumnOrder(5);

        builder.Property(em => em.CreatedAt).IsRequired().HasColumnOrder(6);

        builder.HasOne(em => em.Event).WithMany(e => e.Media).HasForeignKey(em => em.EventId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(em => em.File).WithMany().HasForeignKey(em => em.FileId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(em => em.UploadedByUser).WithMany().HasForeignKey(em => em.UploadedByUserId).OnDelete(DeleteBehavior.SetNull);
    }
}

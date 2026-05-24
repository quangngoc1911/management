using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasColumnOrder(0);

        builder.HasIndex(m => m.MemberId);
        builder.Property(m => m.MemberId).IsRequired().HasColumnOrder(1);

        builder.Property(m => m.RecordType).IsRequired().HasMaxLength(100).HasColumnOrder(2);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(true)
            .HasColumnOrder(3);

        builder.Property(m => m.Diagnosis).HasColumnType("text").HasColumnOrder(4);
        builder.Property(m => m.Treatment).HasColumnType("text").HasColumnOrder(5);
        builder.Property(m => m.DoctorName).HasMaxLength(255).IsUnicode(true).HasColumnOrder(6);
        builder.Property(m => m.HospitalName).HasMaxLength(255).IsUnicode(true).HasColumnOrder(7);

        builder.HasIndex(m => m.RecordDate);
        builder.Property(m => m.RecordDate).IsRequired().HasColumnOrder(8);

        builder.Property(m => m.FollowUpDate).HasColumnOrder(9);

        builder.Property(m => m.IsPrivate).IsRequired().HasDefaultValue(false).HasColumnOrder(10);

        builder.Property(m => m.Notes).HasColumnType("text").HasColumnOrder(11);

        builder.Property(m => m.CreatedByUserId).HasColumnName("created_by").HasColumnOrder(12);
        builder.Property(m => m.UpdatedByUserId).HasColumnName("updated_by").HasColumnOrder(13);
        builder.Property(m => m.DeletedByUserId).HasColumnName("deleted_by").HasColumnOrder(14);

        builder.Property(m => m.CreatedAt).IsRequired().HasColumnOrder(15);
        builder.Property(m => m.UpdatedAt).IsRequired().HasColumnOrder(16);
        builder.Property(m => m.DeletedAt).HasColumnOrder(17);
        builder.Property(m => m.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(18);

        builder.HasOne(m => m.Member).WithMany().HasForeignKey(m => m.MemberId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.CreatedByUser).WithMany().HasForeignKey(m => m.CreatedByUserId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(m => m.UpdatedByUser).WithMany().HasForeignKey(m => m.UpdatedByUserId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(m => m.DeletedByUser).WithMany().HasForeignKey(m => m.DeletedByUserId).OnDelete(DeleteBehavior.SetNull);
    }
}

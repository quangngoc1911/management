using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class MedicationConfiguration : IEntityTypeConfiguration<Medication>
{
    public void Configure(EntityTypeBuilder<Medication> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasColumnOrder(0);

        builder.HasIndex(m => m.MemberId);
        builder.Property(m => m.MemberId).IsRequired().HasColumnOrder(1);

        builder.Property(m => m.MedicalRecordId).HasColumnOrder(2);

        builder.Property(m => m.Name).IsRequired().HasMaxLength(255).IsUnicode(true).HasColumnOrder(3);
        builder.Property(m => m.Dosage).HasMaxLength(100).HasColumnOrder(4);
        builder.Property(m => m.Frequency).HasMaxLength(100).HasColumnOrder(5);
        builder.Property(m => m.StartDate).IsRequired().HasColumnOrder(6);
        builder.Property(m => m.EndDate).HasColumnOrder(7);

        builder.Property(m => m.ReminderTimes).HasColumnType("jsonb").HasColumnOrder(8);

        builder.Property(m => m.IsActive).IsRequired().HasDefaultValue(true).HasColumnOrder(9);
        builder.Property(m => m.Notes).HasColumnType("text").HasColumnOrder(10);

        builder.Property(m => m.CreatedAt).IsRequired().HasColumnOrder(11);

        builder.HasOne(m => m.Member).WithMany().HasForeignKey(m => m.MemberId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.MedicalRecord).WithMany(mr => mr.Medications).HasForeignKey(m => m.MedicalRecordId).OnDelete(DeleteBehavior.SetNull);
    }
}

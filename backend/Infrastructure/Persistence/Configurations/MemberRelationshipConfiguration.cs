using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class MemberRelationshipConfiguration : IEntityTypeConfiguration<MemberRelationship>
{
    public void Configure(EntityTypeBuilder<MemberRelationship> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnOrder(0);

        builder.HasIndex(r => new { r.MemberId, r.RelatedMemberId, r.RelationshipType }).IsUnique();
        builder.HasIndex(r => r.RelationshipType);

        builder.Property(r => r.MemberId).IsRequired().HasColumnOrder(1);
        builder.Property(r => r.RelatedMemberId).IsRequired().HasColumnOrder(2);

        builder.Property(r => r.RelationshipType)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnOrder(3);

        builder.Property(r => r.StartedAt).HasColumnOrder(4);
        builder.Property(r => r.EndedAt).HasColumnOrder(5);
        builder.Property(r => r.IsBiological).HasColumnOrder(6);
        builder.Property(r => r.Notes).HasColumnType("text").HasColumnOrder(7);

        // Audit + soft-delete
        builder.Property(r => r.CreatedAt).IsRequired().HasColumnOrder(8);
        builder.Property(r => r.UpdatedAt).IsRequired().HasColumnOrder(9);
        builder.Property(r => r.DeletedAt).HasColumnOrder(10);
        builder.Property(r => r.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(11);

        // Both FK relationships are configured via FamilyMemberConfiguration inverse mappings
    }
}

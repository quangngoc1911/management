using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using MyAPI.Models;

namespace MyAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<DocumentTag> DocumentTags => Set<DocumentTag>();
    public DbSet<DocumentFile> Files => Set<DocumentFile>();
    public DbSet<DocumentVersion> DocumentVersions => Set<DocumentVersion>();
    public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
    public DbSet<ViewHistory> ViewHistories => Set<ViewHistory>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<SystemConfig> SystemConfigs => Set<SystemConfig>();
    public DbSet<Menu> Menus { get; set; }
    // Phase 2 optional
    public DbSet<DocumentComment> Comments => Set<DocumentComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureGlobalSoftDeleteFilter(modelBuilder);

        // USERS
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.Email).HasMaxLength(256).IsRequired();
            e.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
            e.Property(x => x.Role).HasMaxLength(20).IsRequired();
            e.Property(x => x.AvatarUrl).HasMaxLength(500);
            e.Property(x => x.Department).HasMaxLength(100);
            e.HasIndex(x => x.Email).IsUnique().HasDatabaseName("IX_Users_Email");
            e.HasIndex(x => x.Role).HasDatabaseName("IX_Users_Role");
        });

        // CATEGORIES
        modelBuilder.Entity<Category>(e =>
        {
            e.ToTable("Categories");
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Slug).HasMaxLength(200).IsRequired();
            e.Property(x => x.Description).HasMaxLength(1000);
            e.Property(x => x.Icon).HasMaxLength(100);
            e.Property(x => x.CoverImageUrl).HasMaxLength(500);
            e.HasIndex(x => x.Slug).IsUnique().HasDatabaseName("IX_Categories_Slug");
            e.HasIndex(x => x.ParentId).HasDatabaseName("IX_Categories_ParentId");

            e.HasOne(x => x.Parent)
             .WithMany(x => x.Children)
             .HasForeignKey(x => x.ParentId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // DOCUMENTS
        modelBuilder.Entity<Document>(e =>
        {
            e.ToTable("Documents");
            e.Property(x => x.Title).HasMaxLength(500).IsRequired();
            e.Property(x => x.Slug).HasMaxLength(500).IsRequired();
            e.Property(x => x.Summary).HasMaxLength(2000);
            e.Property(x => x.ContentType).HasMaxLength(10).IsRequired();
            e.Property(x => x.ThumbnailUrl).HasMaxLength(500);

            e.HasIndex(x => x.Slug).IsUnique().HasDatabaseName("IX_Documents_Slug");
            e.HasIndex(x => x.CategoryId).HasDatabaseName("IX_Documents_CategoryId");
            e.HasIndex(x => x.IsPublished).HasDatabaseName("IX_Documents_IsPublished");
            e.HasIndex(x => x.CreatedByUserId).HasDatabaseName("IX_Documents_CreatedByUserId");
            e.HasIndex(x => x.ContentType).HasDatabaseName("IX_Documents_ContentType");
            e.HasIndex(x => new { x.CategoryId, x.IsPublished, x.IsDeleted })
             .HasDatabaseName("IX_Documents_Category_Published_Deleted");

            e.HasOne(x => x.Category)
             .WithMany(x => x.Documents)
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.CreatedByUser)
             .WithMany(x => x.CreatedDocuments)
             .HasForeignKey(x => x.CreatedByUserId)
             .OnDelete(DeleteBehavior.Restrict);

            // Optional many-to-one; schema có FileId ở Documents
            e.HasOne(x => x.File)
             .WithMany(x => x.Documents)
             .HasForeignKey(x => x.FileId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // TAGS
        modelBuilder.Entity<Tag>(e =>
        {
            e.ToTable("Tags");
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.Slug).HasMaxLength(100).IsRequired();
            e.Property(x => x.Color).HasMaxLength(7);
            e.HasIndex(x => x.Slug).IsUnique().HasDatabaseName("IX_Tags_Slug");
        });

        // DOCUMENT TAGS
        modelBuilder.Entity<DocumentTag>(e =>
        {
            e.ToTable("DocumentTags");
            e.HasKey(x => new { x.DocumentId, x.TagId });

            e.Property(x => x.AddedAt).HasDefaultValueSql("NOW()");

            e.HasOne(x => x.Document)
             .WithMany(x => x.DocumentTags)
             .HasForeignKey(x => x.DocumentId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Tag)
             .WithMany(x => x.DocumentTags)
             .HasForeignKey(x => x.TagId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // FILES
        modelBuilder.Entity<DocumentFile>(e =>
        {
            e.ToTable("Files");
            e.Property(x => x.OriginalName).HasMaxLength(500).IsRequired();
            e.Property(x => x.StoredName).HasMaxLength(500).IsRequired();
            e.Property(x => x.StoragePath).HasMaxLength(1000).IsRequired();
            e.Property(x => x.PublicUrl).HasMaxLength(1000).IsRequired();
            e.Property(x => x.FileType).HasMaxLength(20).IsRequired();
            e.Property(x => x.MimeType).HasMaxLength(100).IsRequired();
            e.Property(x => x.StorageProvider).HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.UploadedByUserId).HasDatabaseName("IX_Files_UploadedByUserId");

            e.HasOne(x => x.UploadedByUser)
             .WithMany(x => x.UploadedFiles)
             .HasForeignKey(x => x.UploadedByUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // DOCUMENT VERSIONS
        modelBuilder.Entity<DocumentVersion>(e =>
        {
            e.ToTable("DocumentVersions");
            e.Property(x => x.Title).HasMaxLength(500).IsRequired();
            e.Property(x => x.ChangeSummary).HasMaxLength(500);

            e.HasOne(x => x.Document)
             .WithMany(x => x.Versions)
             .HasForeignKey(x => x.DocumentId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.EditedByUser)
             .WithMany(x => x.EditedVersions)
             .HasForeignKey(x => x.EditedByUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // BOOKMARKS
        modelBuilder.Entity<Bookmark>(e =>
        {
            e.ToTable("Bookmarks");
            e.HasKey(x => new { x.UserId, x.DocumentId });
            e.Property(x => x.Note).HasMaxLength(500);
            e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");

            e.HasOne(x => x.User)
             .WithMany(x => x.Bookmarks)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Document)
             .WithMany(x => x.Bookmarks)
             .HasForeignKey(x => x.DocumentId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // VIEW HISTORIES
        modelBuilder.Entity<ViewHistory>(e =>
        {
            e.ToTable("ViewHistories");
            e.Property(x => x.ViewedAt).HasDefaultValueSql("NOW()");
            e.HasIndex(x => new { x.UserId, x.ViewedAt })
             .HasDatabaseName("IX_ViewHistories_User_ViewedAt");

            e.HasOne(x => x.User)
             .WithMany(x => x.ViewHistories)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Document)
             .WithMany(x => x.ViewHistories)
             .HasForeignKey(x => x.DocumentId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // REFRESH TOKENS
        modelBuilder.Entity<RefreshToken>(e =>
        {
            e.ToTable("RefreshTokens");
            e.Property(x => x.TokenHash).HasMaxLength(500).IsRequired();
            e.Property(x => x.DeviceInfo).HasMaxLength(300);
            e.HasIndex(x => x.UserId).HasDatabaseName("IX_RefreshTokens_UserId");
            e.HasIndex(x => x.TokenHash).IsUnique().HasDatabaseName("IX_RefreshTokens_Hash");

            e.HasOne(x => x.User)
             .WithMany(x => x.RefreshTokens)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // SYSTEM CONFIGS
        modelBuilder.Entity<SystemConfig>(e =>
        {
            e.ToTable("SystemConfigs");
            e.Property(x => x.Key).HasMaxLength(100).IsRequired();
            e.Property(x => x.Description).HasMaxLength(300);
            e.HasIndex(x => x.Key).IsUnique().HasDatabaseName("IX_SystemConfigs_Key");
        });

        // COMMENTS - Phase 2 optional
        modelBuilder.Entity<DocumentComment>(e =>
        {
            e.ToTable("Comments");
            e.Property(x => x.Content).IsRequired();

            e.HasOne(x => x.Document)
             .WithMany()
             .HasForeignKey(x => x.DocumentId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.User)
             .WithMany()
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // MENUS
        modelBuilder.Entity<Menu>(e =>
        {
            e.ToTable("Menus");
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.Path).HasMaxLength(200);
            e.Property(x => x.Icon).HasMaxLength(100);

            e.HasIndex(x => x.ParentId).HasDatabaseName("IX_Menus_ParentId");

            e.HasOne(m => m.Parent)
             .WithMany(m => m.Children)
             .HasForeignKey(m => m.ParentId)
             .OnDelete(DeleteBehavior.Restrict);  // ✅ thêm OnDelete
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditFields();
        return base.SaveChanges();
    }

    private void ApplyAuditFields()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.IsDeleted = false;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = now;
                entry.Entity.UpdatedAt = now;
            }
        }
    }

    private static void ConfigureGlobalSoftDeleteFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var body = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(body, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}
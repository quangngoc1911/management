using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Menus.Domain.Entities;
using ManagementSystem.Modules.Categories.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;
using ManagementSystem.Infrastructure.Persistence.Configurations;

namespace ManagementSystem.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<RoleMenu> RoleMenus => Set<RoleMenu>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentField> DocumentFields => Set<DocumentField>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<DocumentTag> DocumentTags => Set<DocumentTag>();
    public DbSet<DocumentVersion> DocumentVersions => Set<DocumentVersion>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<DocumentShare> DocumentShares => Set<DocumentShare>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<ViewHistory> ViewHistories => Set<ViewHistory>();
    public DbSet<DocumentFile> Files => Set<DocumentFile>();
    public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
    public DbSet<SystemConfig> SystemConfigs => Set<SystemConfig>();
    public DbSet<DocumentComment> DocumentComments => Set<DocumentComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global Soft Delete Filter
        ConfigureGlobalSoftDeleteFilter(modelBuilder);

        // Entities Configurations
        ConfigureEntities(modelBuilder);
        // ✅ Thêm: Convert tên bảng và cột sang snake_case
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Users → users, DocumentTags → document_tags
            entity.SetTableName(ToSnakeCase(entity.GetTableName()!));

            // PasswordHash → password_hash, CreatedAt → created_at
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            }

            // Foreign key columns
            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(ToSnakeCase(key.GetConstraintName()!));
            }
        }
    }

    private void ConfigureEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new MenuConfiguration());
        modelBuilder.ApplyConfiguration(new RoleMenuConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentFieldConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentTagConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentVersionConfiguration());
        modelBuilder.ApplyConfiguration(new AttachmentConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentShareConfiguration());
        modelBuilder.ApplyConfiguration(new FavoriteConfiguration());
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
        modelBuilder.ApplyConfiguration(new SettingConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
        modelBuilder.ApplyConfiguration(new BookmarkConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentFileConfiguration());
        modelBuilder.ApplyConfiguration(new ViewHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new SystemConfigConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentCommentConfiguration());
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

    private static string ToSnakeCase(string name)
    {
        return string.Concat(
            name.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c : c.ToString())
        ).ToLower();
    }
}

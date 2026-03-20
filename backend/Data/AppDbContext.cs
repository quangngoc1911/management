using Microsoft.EntityFrameworkCore;

using MyAPI.Models;

namespace MyAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Categories> Categories { get; set; }

    public DbSet<Documents> Documents { get; set; }
    public DbSet<Tags> Tags { get; set; }
    public DbSet<DocumentTags> DocumentTags { get; set; }
    public DbSet<DocumentVersions> DocumentVersions { get; set; }
    public DbSet<Files> Files { get; set; }
    public DbSet<Bookmarks> Bookmarks { get; set; }
    public DbSet<ViewHistories> ViewHistories { get; set; }
    public DbSet<RefreshTokens> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // many-to-many PK
        modelBuilder.Entity<DocumentTags>()
            .HasKey(dt => new { dt.DocumentId, dt.TagId });

        // self reference Category
        modelBuilder.Entity<Categories>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId);

        // one-to-one Document - File
        modelBuilder.Entity<Files>()
            .HasOne(f => f.Document)
            .WithOne()
            .HasForeignKey<Files>(f => f.DocumentId);
    }
}
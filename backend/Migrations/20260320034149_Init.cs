using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoriesName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Slug = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoriesId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    AvatarUrl = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastLoginAt = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

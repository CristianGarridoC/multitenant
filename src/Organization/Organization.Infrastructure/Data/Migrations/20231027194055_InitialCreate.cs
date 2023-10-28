using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Organization.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "multi_tenant");

            migrationBuilder.CreateTable(
                name: "organization",
                schema: "multi_tenant",
                columns: table => new
                {
                    id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    slug_tenant = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "multi_tenant",
                columns: table => new
                {
                    id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_organization",
                schema: "multi_tenant",
                columns: table => new
                {
                    id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organization_id = table.Column<int>(type: "INT", nullable: false),
                    user_id = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_organization", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_organization_organization_organization_id",
                        column: x => x.organization_id,
                        principalSchema: "multi_tenant",
                        principalTable: "organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_organization_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "multi_tenant",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_organization_slug_tenant_name",
                schema: "multi_tenant",
                table: "organization",
                columns: new[] { "slug_tenant", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_email",
                schema: "multi_tenant",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_organization_organization_id_user_id",
                schema: "multi_tenant",
                table: "user_organization",
                columns: new[] { "organization_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_organization_user_id",
                schema: "multi_tenant",
                table: "user_organization",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_organization",
                schema: "multi_tenant");

            migrationBuilder.DropTable(
                name: "organization",
                schema: "multi_tenant");

            migrationBuilder.DropTable(
                name: "user",
                schema: "multi_tenant");
        }
    }
}

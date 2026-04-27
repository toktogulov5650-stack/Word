using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Word.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWordTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordId = table.Column<int>(type: "integer", nullable: false),
                    KyrgyzWord = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordTranslations_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordTranslations_WordId",
                table: "WordTranslations",
                column: "WordId");

            migrationBuilder.Sql(
                """
                INSERT INTO "WordTranslations" ("WordId", "KyrgyzWord")
                SELECT "Id", "KyrgyzWord"
                FROM "Words"
                WHERE "KyrgyzWord" IS NOT NULL AND btrim("KyrgyzWord") <> '';
                """);

            migrationBuilder.DropColumn(
                name: "KyrgyzWord",
                table: "Words");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordTranslations");

            migrationBuilder.AddColumn<string>(
                name: "KyrgyzWord",
                table: "Words",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                """
                UPDATE "Words" AS w
                SET "KyrgyzWord" = sub."KyrgyzWord"
                FROM (
                    SELECT DISTINCT ON ("WordId") "WordId", "KyrgyzWord"
                    FROM "WordTranslations"
                    ORDER BY "WordId", "Id"
                ) AS sub
                WHERE w."Id" = sub."WordId";
                """);
        }
    }
}

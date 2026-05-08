using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Word.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorLocalizationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WordTranslations_WordId",
                table: "WordTranslations");

            migrationBuilder.RenameColumn(
                name: "KyrgyzWord",
                table: "WordTranslations",
                newName: "Text");

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "WordTranslations",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "ky");

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "TestSessions",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "ru");

            migrationBuilder.CreateTable(
                name: "CategoryTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryTranslations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordExamples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordExplanationId = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordExamples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordExamples_WordExplanations_WordExplanationId",
                        column: x => x.WordExplanationId,
                        principalTable: "WordExplanations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordExplanationTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordExplanationId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    WhatIs = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Translations = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Usage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Hint = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordExplanationTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordExplanationTranslations_WordExplanations_WordExplanatio~",
                        column: x => x.WordExplanationId,
                        principalTable: "WordExplanations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordExampleTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordExampleId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Translation = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordExampleTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordExampleTranslations_WordExamples_WordExampleId",
                        column: x => x.WordExampleId,
                        principalTable: "WordExamples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(
                """
                INSERT INTO "CategoryTranslations" ("CategoryId", "LanguageCode", "Name", "Description")
                SELECT "Id", 'ky', COALESCE("Name", ''), COALESCE("Description", '')
                FROM "Categories";

                INSERT INTO "CategoryTranslations" ("CategoryId", "LanguageCode", "Name", "Description")
                SELECT "Id", 'ru', COALESCE("Name", ''), COALESCE("Description", '')
                FROM "Categories";
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO "WordExplanationTranslations" ("WordExplanationId", "LanguageCode", "WhatIs", "Meaning", "Translations", "Usage", "Hint")
                SELECT "Id", 'ky',
                       COALESCE("WhatIs", ''),
                       COALESCE("Meaning", ''),
                       COALESCE("Translations", ''),
                       COALESCE("Usage", ''),
                       COALESCE("Hint", '')
                FROM "WordExplanations";

                INSERT INTO "WordExplanationTranslations" ("WordExplanationId", "LanguageCode", "WhatIs", "Meaning", "Translations", "Usage", "Hint")
                SELECT "Id", 'ru',
                       COALESCE("WhatIs", ''),
                       COALESCE("Meaning", ''),
                       COALESCE("Translations", ''),
                       COALESCE("Usage", ''),
                       COALESCE("Hint", '')
                FROM "WordExplanations";
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO "WordExamples" ("WordExplanationId", "SortOrder")
                SELECT "Id", 1 FROM "WordExplanations"
                UNION ALL
                SELECT "Id", 2 FROM "WordExplanations"
                UNION ALL
                SELECT "Id", 3 FROM "WordExplanations";
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO "WordExampleTranslations" ("WordExampleId", "LanguageCode", "Text", "Translation")
                SELECT we."Id",
                       'ky',
                       CASE we."SortOrder"
                           WHEN 1 THEN COALESCE(weo."Example1", '')
                           WHEN 2 THEN COALESCE(weo."Example2", '')
                           ELSE COALESCE(weo."Example3", '')
                       END,
                       CASE we."SortOrder"
                           WHEN 1 THEN COALESCE(weo."Example1Translation", '')
                           WHEN 2 THEN COALESCE(weo."Example2Translation", '')
                           ELSE COALESCE(weo."Example3Translation", '')
                       END
                FROM "WordExamples" we
                INNER JOIN "WordExplanations" weo ON weo."Id" = we."WordExplanationId";

                INSERT INTO "WordExampleTranslations" ("WordExampleId", "LanguageCode", "Text", "Translation")
                SELECT we."Id",
                       'ru',
                       CASE we."SortOrder"
                           WHEN 1 THEN COALESCE(weo."Example1", '')
                           WHEN 2 THEN COALESCE(weo."Example2", '')
                           ELSE COALESCE(weo."Example3", '')
                       END,
                       CASE we."SortOrder"
                           WHEN 1 THEN COALESCE(weo."Example1Translation", '')
                           WHEN 2 THEN COALESCE(weo."Example2Translation", '')
                           ELSE COALESCE(weo."Example3Translation", '')
                       END
                FROM "WordExamples" we
                INNER JOIN "WordExplanations" weo ON weo."Id" = we."WordExplanationId";
                """);

            migrationBuilder.DropColumn(
                name: "Example1",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Example1Translation",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Example2",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Example2Translation",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Example3",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Example3Translation",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Hint",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Meaning",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Translations",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Usage",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "WhatIs",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_WordTranslations_WordId_LanguageCode_Text",
                table: "WordTranslations",
                columns: new[] { "WordId", "LanguageCode", "Text" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTranslations_CategoryId_LanguageCode",
                table: "CategoryTranslations",
                columns: new[] { "CategoryId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordExamples_WordExplanationId_SortOrder",
                table: "WordExamples",
                columns: new[] { "WordExplanationId", "SortOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordExampleTranslations_WordExampleId_LanguageCode",
                table: "WordExampleTranslations",
                columns: new[] { "WordExampleId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordExplanationTranslations_WordExplanationId_LanguageCode",
                table: "WordExplanationTranslations",
                columns: new[] { "WordExplanationId", "LanguageCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WordTranslations_WordId_LanguageCode_Text",
                table: "WordTranslations");

            migrationBuilder.AddColumn<string>(
                name: "Example1",
                table: "WordExplanations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Example1Translation",
                table: "WordExplanations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Example2",
                table: "WordExplanations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Example2Translation",
                table: "WordExplanations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Example3",
                table: "WordExplanations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Example3Translation",
                table: "WordExplanations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Hint",
                table: "WordExplanations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Meaning",
                table: "WordExplanations",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Translations",
                table: "WordExplanations",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Usage",
                table: "WordExplanations",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WhatIs",
                table: "WordExplanations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Categories",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                """
                UPDATE "Categories" c
                SET "Name" = COALESCE(ct."Name", ''),
                    "Description" = COALESCE(ct."Description", '')
                FROM (
                    SELECT DISTINCT ON ("CategoryId")
                        "CategoryId",
                        "Name",
                        "Description"
                    FROM "CategoryTranslations"
                    ORDER BY "CategoryId",
                             CASE WHEN "LanguageCode" = 'ky' THEN 0 WHEN "LanguageCode" = 'ru' THEN 1 ELSE 2 END
                ) ct
                WHERE c."Id" = ct."CategoryId";

                UPDATE "WordExplanations" we
                SET "WhatIs" = COALESCE(wet."WhatIs", ''),
                    "Meaning" = COALESCE(wet."Meaning", ''),
                    "Translations" = COALESCE(wet."Translations", ''),
                    "Usage" = COALESCE(wet."Usage", ''),
                    "Hint" = COALESCE(wet."Hint", '')
                FROM (
                    SELECT DISTINCT ON ("WordExplanationId")
                        "WordExplanationId",
                        "WhatIs",
                        "Meaning",
                        "Translations",
                        "Usage",
                        "Hint"
                    FROM "WordExplanationTranslations"
                    ORDER BY "WordExplanationId",
                             CASE WHEN "LanguageCode" = 'ky' THEN 0 WHEN "LanguageCode" = 'ru' THEN 1 ELSE 2 END
                ) wet
                WHERE we."Id" = wet."WordExplanationId";

                UPDATE "WordExplanations" we
                SET "Example1" = COALESCE(t1."Text", ''),
                    "Example1Translation" = COALESCE(t1."Translation", ''),
                    "Example2" = COALESCE(t2."Text", ''),
                    "Example2Translation" = COALESCE(t2."Translation", ''),
                    "Example3" = COALESCE(t3."Text", ''),
                    "Example3Translation" = COALESCE(t3."Translation", '')
                FROM (
                    SELECT DISTINCT ON (we."WordExplanationId")
                        we."WordExplanationId", wet."Text", wet."Translation"
                    FROM "WordExamples" we
                    JOIN "WordExampleTranslations" wet ON wet."WordExampleId" = we."Id"
                    WHERE we."SortOrder" = 1
                    ORDER BY we."WordExplanationId",
                             CASE WHEN wet."LanguageCode" = 'ky' THEN 0 WHEN wet."LanguageCode" = 'ru' THEN 1 ELSE 2 END
                ) t1,
                (
                    SELECT DISTINCT ON (we."WordExplanationId")
                        we."WordExplanationId", wet."Text", wet."Translation"
                    FROM "WordExamples" we
                    JOIN "WordExampleTranslations" wet ON wet."WordExampleId" = we."Id"
                    WHERE we."SortOrder" = 2
                    ORDER BY we."WordExplanationId",
                             CASE WHEN wet."LanguageCode" = 'ky' THEN 0 WHEN wet."LanguageCode" = 'ru' THEN 1 ELSE 2 END
                ) t2,
                (
                    SELECT DISTINCT ON (we."WordExplanationId")
                        we."WordExplanationId", wet."Text", wet."Translation"
                    FROM "WordExamples" we
                    JOIN "WordExampleTranslations" wet ON wet."WordExampleId" = we."Id"
                    WHERE we."SortOrder" = 3
                    ORDER BY we."WordExplanationId",
                             CASE WHEN wet."LanguageCode" = 'ky' THEN 0 WHEN wet."LanguageCode" = 'ru' THEN 1 ELSE 2 END
                ) t3
                WHERE we."Id" = t1."WordExplanationId"
                  AND we."Id" = t2."WordExplanationId"
                  AND we."Id" = t3."WordExplanationId";
                """);

            migrationBuilder.DropTable(
                name: "CategoryTranslations");

            migrationBuilder.DropTable(
                name: "WordExampleTranslations");

            migrationBuilder.DropTable(
                name: "WordExplanationTranslations");

            migrationBuilder.DropTable(
                name: "WordExamples");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "WordTranslations");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "TestSessions");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "WordTranslations",
                newName: "KyrgyzWord");

            migrationBuilder.CreateIndex(
                name: "IX_WordTranslations_WordId",
                table: "WordTranslations",
                column: "WordId");
        }
    }
}

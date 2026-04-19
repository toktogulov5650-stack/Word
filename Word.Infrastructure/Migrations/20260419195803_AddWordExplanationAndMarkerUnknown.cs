using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Word.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWordExplanationAndMarkerUnknown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMarkedUnknown",
                table: "TestQuestions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "WordExplanations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordId = table.Column<int>(type: "integer", nullable: false),
                    WhatIs = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Meaning = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Translations = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Usage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Example1 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Example2 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Example3 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Hint = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordExplanations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordExplanations_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordExplanations_WordId",
                table: "WordExplanations",
                column: "WordId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "IsMarkedUnknown",
                table: "TestQuestions");
        }
    }
}

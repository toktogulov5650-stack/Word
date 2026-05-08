using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Word.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryImageAndExampleTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Example1Translation",
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
                name: "Example3Translation",
                table: "WordExplanations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Categories",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Example1Translation",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Example2Translation",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "Example3Translation",
                table: "WordExplanations");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Categories");
        }
    }
}

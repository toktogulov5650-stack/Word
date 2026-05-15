using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Word.Infrastructure.Persistence;

#nullable disable

namespace Word.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260515113500_AddCategoryRecordTotalQuestionCount")]
    public partial class AddCategoryRecordTotalQuestionCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BestTotalQuestionCount",
                table: "CategoryRecord",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BestTotalQuestionCount",
                table: "CategoryRecord");
        }
    }
}

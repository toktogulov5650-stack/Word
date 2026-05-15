using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Word.Infrastructure.Persistence;

#nullable disable

namespace Word.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260515115000_BackfillCategoryRecordTotalQuestionCount")]
    public partial class BackfillCategoryRecordTotalQuestionCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE "CategoryRecord" cr
                SET "BestTotalQuestionCount" = COALESCE(
                    (
                        SELECT ts."TotalQuestionCount"
                        FROM "TestSessions" ts
                        WHERE ts."CategoryId" = cr."CategoryId"
                          AND ts."Status" = 2
                          AND ts."CorrectAnswerCount" = cr."BestScore"
                          AND ts."TotalQuestionCount" > 0
                        ORDER BY ts."Id" DESC
                        LIMIT 1
                    ),
                    (
                        SELECT GREATEST(COUNT(*)::integer, cr."BestScore")
                        FROM "Words" w
                        WHERE w."CategoryId" = cr."CategoryId"
                          AND w."IsActive" = TRUE
                          AND EXISTS (
                              SELECT 1
                              FROM "WordTranslations" wt
                              WHERE wt."WordId" = w."Id"
                          )
                    ),
                    cr."BestTotalQuestionCount"
                )
                WHERE cr."BestTotalQuestionCount" = 0;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE "CategoryRecord"
                SET "BestTotalQuestionCount" = 0;
                """);
        }
    }
}

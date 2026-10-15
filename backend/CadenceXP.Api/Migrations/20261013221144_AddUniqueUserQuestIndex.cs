using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueUserQuestIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserQuests_UserId",
                table: "UserQuests");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuests_UserId_QuestId",
                table: "UserQuests",
                columns: new[] { "UserId", "QuestId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserQuests_UserId_QuestId",
                table: "UserQuests");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuests_UserId",
                table: "UserQuests",
                column: "UserId");
        }
    }
}

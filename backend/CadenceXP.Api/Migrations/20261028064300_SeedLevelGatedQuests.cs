using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedLevelGatedQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "Difficulty", "Name", "RequiredLevel", "TargetValue", "Type", "XpReward" },
                values: new object[,]
                {
                    { 4, "Complete a single ride of at least 3 miles.", 0, "Ride 3 Miles in One Ride", 3, 3.0, 3, 100 },
                    { 5, "Ride a total of 15 miles after unlocking this quest.", 0, "Ride 15 Total Miles", 5, 15.0, 0, 150 },
                    { 6, "Climb a total of 500 meters after unlocking this quest.", 0, "Climb 500 Meters", 5, 500.0, 1, 175 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}

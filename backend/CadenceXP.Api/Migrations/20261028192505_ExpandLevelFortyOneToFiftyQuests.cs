using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpandLevelFortyOneToFiftyQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "Difficulty", "Name", "RequiredLevel", "TargetValue", "Type", "XpReward" },
                values: new object[,]
                {
                    { 41, "Ride a total of 300 miles after unlocking this quest.", 0, "Ride 300 Total Miles", 42, 300.0, 0, 500 },
                    { 42, "Complete 90 rides after unlocking this quest.", 0, "Complete 90 Rides", 42, 90.0, 2, 500 },
                    { 43, "Climb a total of 6,500 meters after unlocking this quest.", 0, "Climb 6,500 Meters", 43, 6500.0, 1, 500 },
                    { 44, "Complete a single ride of at least 45 miles.", 1, "Ride 45 Miles in One Ride", 45, 45.0, 3, 650 },
                    { 45, "Complete 100 rides after unlocking this quest.", 0, "Complete 100 Rides", 45, 100.0, 2, 550 },
                    { 46, "Ride a total of 400 miles after unlocking this quest.", 0, "Ride 400 Total Miles", 47, 400.0, 0, 600 },
                    { 47, "Climb a total of 8,000 meters after unlocking this quest.", 0, "Climb 8,000 Meters", 48, 8000.0, 1, 600 },
                    { 48, "Complete a single ride of at least 100 miles.", 2, "Century Ride", 48, 100.0, 3, 1500 },
                    { 49, "Complete 125 rides after unlocking this quest.", 0, "Complete 125 Rides", 50, 125.0, 2, 650 },
                    { 50, "Ride a total of 500 miles after unlocking this quest.", 0, "Ride 500 Total Miles", 50, 500.0, 0, 700 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 50);
        }
    }
}

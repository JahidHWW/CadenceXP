using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpandLevelTwentyOneToThirtyQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "Difficulty", "Name", "RequiredLevel", "TargetValue", "Type", "XpReward" },
                values: new object[,]
                {
                    { 22, "Ride a total of 100 miles after unlocking this quest.", 0, "Ride 100 Total Miles", 22, 100.0, 0, 300 },
                    { 23, "Complete 25 rides after unlocking this quest.", 0, "Complete 25 Rides", 22, 25.0, 2, 250 },
                    { 24, "Climb a total of 2,000 meters after unlocking this quest.", 0, "Climb 2,000 Meters", 23, 2000.0, 1, 300 },
                    { 25, "Complete a single ride of at least 25 miles.", 1, "Ride 25 Miles in One Ride", 25, 25.0, 3, 400 },
                    { 26, "Complete 30 rides after unlocking this quest.", 0, "Complete 30 Rides", 25, 30.0, 2, 300 },
                    { 27, "Ride a total of 150 miles after unlocking this quest.", 0, "Ride 150 Total Miles", 27, 150.0, 0, 350 },
                    { 28, "Climb a total of 3,000 meters after unlocking this quest.", 0, "Climb 3,000 Meters", 28, 3000.0, 1, 350 },
                    { 29, "Complete 40 rides after unlocking this quest.", 0, "Complete 40 Rides", 30, 40.0, 2, 350 },
                    { 30, "Complete a single ride of at least 30 miles.", 1, "Ride 30 Miles in One Ride", 30, 30.0, 3, 450 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 30);
        }
    }
}

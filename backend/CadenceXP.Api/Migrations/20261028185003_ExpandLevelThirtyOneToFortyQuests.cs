using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpandLevelThirtyOneToFortyQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "Difficulty", "Name", "RequiredLevel", "TargetValue", "Type", "XpReward" },
                values: new object[,]
                {
                    { 31, "Ride a total of 200 miles after unlocking this quest.", 0, "Ride 200 Total Miles", 32, 200.0, 0, 400 },
                    { 32, "Complete 50 rides after unlocking this quest.", 0, "Complete 50 Rides", 32, 50.0, 2, 400 },
                    { 33, "Climb a total of 4,000 meters after unlocking this quest.", 0, "Climb 4,000 Meters", 33, 4000.0, 1, 400 },
                    { 34, "Complete a single ride of at least 35 miles.", 1, "Ride 35 Miles in One Ride", 35, 35.0, 3, 500 },
                    { 35, "Complete 60 rides after unlocking this quest.", 0, "Complete 60 Rides", 35, 60.0, 2, 400 },
                    { 36, "Ride a total of 250 miles after unlocking this quest.", 0, "Ride 250 Total Miles", 37, 250.0, 0, 450 },
                    { 37, "Climb a total of 5,000 meters after unlocking this quest.", 0, "Climb 5,000 Meters", 38, 5000.0, 1, 450 },
                    { 38, "Complete 75 rides after unlocking this quest.", 0, "Complete 75 Rides", 40, 75.0, 2, 450 },
                    { 39, "Complete a single ride of at least 40 miles.", 1, "Ride 40 Miles in One Ride", 40, 40.0, 3, 600 },
                    { 40, "Complete a single ride of at least 50 miles.", 2, "The Fifty", 40, 50.0, 3, 900 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 40);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpandLevelElevenToTwentyQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "Difficulty", "Name", "RequiredLevel", "TargetValue", "Type", "XpReward" },
                values: new object[,]
                {
                    { 13, "Ride a total of 50 miles after unlocking this quest.", 0, "Ride 50 Total Miles", 12, 50.0, 0, 200 },
                    { 14, "Complete 12 rides after unlocking this quest.", 0, "Complete 12 Rides", 12, 12.0, 2, 175 },
                    { 15, "Climb a total of 1,000 meters after unlocking this quest.", 0, "Climb 1,000 Meters", 13, 1000.0, 1, 200 },
                    { 16, "Complete a single ride of at least 15 miles.", 1, "Ride 15 Miles in One Ride", 15, 15.0, 3, 300 },
                    { 17, "Complete 15 rides after unlocking this quest.", 0, "Complete 15 Rides", 15, 15.0, 2, 200 },
                    { 18, "Ride a total of 75 miles after unlocking this quest.", 0, "Ride 75 Total Miles", 17, 75.0, 0, 250 },
                    { 19, "Climb a total of 1,500 meters after unlocking this quest.", 0, "Climb 1,500 Meters", 18, 1500.0, 1, 250 },
                    { 20, "Complete 20 rides after unlocking this quest.", 0, "Complete 20 Rides", 20, 20.0, 2, 250 },
                    { 21, "Complete a single ride of at least 20 miles.", 1, "Ride 20 Miles in One Ride", 20, 20.0, 3, 350 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 21);
        }
    }
}

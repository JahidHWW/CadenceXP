using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class ExpandLevelOneToTenQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "XpReward",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "XpReward",
                value: 60);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "XpReward",
                value: 75);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "XpReward",
                value: 75);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5,
                column: "XpReward",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 6,
                column: "XpReward",
                value: 100);

            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "Difficulty", "Name", "RequiredLevel", "TargetValue", "Type", "XpReward" },
                values: new object[,]
                {
                    { 7, "Complete 5 rides after unlocking this quest.", 0, "Complete 5 Rides", 7, 5.0, 2, 100 },
                    { 8, "Complete a single ride of at least 5 miles.", 0, "Ride 5 Miles in One Ride", 7, 5.0, 3, 125 },
                    { 9, "Climb a total of 750 meters after unlocking this quest.", 0, "Climb 750 Meters", 8, 750.0, 1, 125 },
                    { 10, "Ride a total of 30 miles after unlocking this quest.", 0, "Ride 30 Total Miles", 10, 30.0, 0, 150 },
                    { 11, "Complete 10 rides after unlocking this quest.", 0, "Complete 10 Rides", 10, 10.0, 2, 150 },
                    { 12, "Complete a single ride of at least 10 miles.", 1, "Ride 10 Miles in One Ride", 10, 10.0, 3, 225 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                column: "XpReward",
                value: 75);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                column: "XpReward",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                column: "XpReward",
                value: 125);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 4,
                column: "XpReward",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 5,
                column: "XpReward",
                value: 150);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 6,
                column: "XpReward",
                value: 175);
        }
    }
}

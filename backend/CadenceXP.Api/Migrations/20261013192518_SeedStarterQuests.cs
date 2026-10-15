using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedStarterQuests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quests",
                columns: new[] { "Id", "Description", "Name", "TargetValue", "Type", "XpReward" },
                values: new object[,]
                {
                    { 1, "Ride a total of 5 miles.", "Ride 5 Miles", 5.0, 0, 75 },
                    { 2, "Climb a total of 300 meters.", "Climb 300 Meters", 300.0, 1, 100 },
                    { 3, "Complete your first 3 rides.", "Complete 3 Rides", 3.0, 2, 125 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}

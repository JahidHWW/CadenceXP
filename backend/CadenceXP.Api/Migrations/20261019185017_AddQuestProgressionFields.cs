using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestProgressionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UnlockedAtUtc",
                table: "UserQuests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Quests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequiredLevel",
                table: "Quests",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Difficulty", "RequiredLevel" },
                values: new object[] { 0, 1 });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Difficulty", "RequiredLevel" },
                values: new object[] { 0, 1 });

            migrationBuilder.UpdateData(
                table: "Quests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Difficulty", "RequiredLevel" },
                values: new object[] { 0, 1 });

            migrationBuilder.Sql(
                """
                UPDATE "UserQuests" AS uq
                SET "UnlockedAtUtc" = COALESCE(
                    (
                        SELECT MIN(r."RideDateUtc")
                        FROM "Rides" AS r
                        WHERE r."UserId" = uq."UserId"
                        AND r."ProcessingStatus" = 2
                    ),
                    uq."CompletedAtUtc",
                    NOW()
                )
                WHERE uq."UnlockedAtUtc" IS NULL;
                """
            );

            migrationBuilder.AlterColumn<DateTime>(
                name: "UnlockedAtUtc",
                table: "UserQuests",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnlockedAtUtc",
                table: "UserQuests");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "RequiredLevel",
                table: "Quests");
        }
    }
}
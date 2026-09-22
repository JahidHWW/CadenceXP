using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersBikesAndRideDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RideDate",
                table: "Rides",
                newName: "RideDateUtc");

            migrationBuilder.RenameColumn(
                name: "DistanceMiles",
                table: "Rides",
                newName: "ElevationGainMeters");

            migrationBuilder.AddColumn<int>(
                name: "BikeId",
                table: "Rides",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "DistanceMeters",
                table: "Rides",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DurationSeconds",
                table: "Rides",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Rides",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Bikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rides_BikeId",
                table: "Rides",
                column: "BikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_UserId",
                table: "Rides",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_UserId",
                table: "Bikes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Bikes_BikeId",
                table: "Rides",
                column: "BikeId",
                principalTable: "Bikes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Users_UserId",
                table: "Rides",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Bikes_BikeId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Users_UserId",
                table: "Rides");

            migrationBuilder.DropTable(
                name: "Bikes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Rides_BikeId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_UserId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "BikeId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "DistanceMeters",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "DurationSeconds",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rides");

            migrationBuilder.RenameColumn(
                name: "RideDateUtc",
                table: "Rides",
                newName: "RideDate");

            migrationBuilder.RenameColumn(
                name: "ElevationGainMeters",
                table: "Rides",
                newName: "DistanceMiles");
        }
    }
}

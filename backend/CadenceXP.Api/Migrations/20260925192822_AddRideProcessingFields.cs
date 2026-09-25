using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadenceXP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRideProcessingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "Rides",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessingStatus",
                table: "Rides",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "ProcessingStatus",
                table: "Rides");
        }
    }
}

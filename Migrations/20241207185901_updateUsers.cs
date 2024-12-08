using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class updateUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "adress",
                table: "ParticulierHuurders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "postcode",
                table: "ParticulierHuurders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "telefoonnummer",
                table: "ParticulierHuurders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "woonplaats",
                table: "ParticulierHuurders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adress",
                table: "ParticulierHuurders");

            migrationBuilder.DropColumn(
                name: "postcode",
                table: "ParticulierHuurders");

            migrationBuilder.DropColumn(
                name: "telefoonnummer",
                table: "ParticulierHuurders");

            migrationBuilder.DropColumn(
                name: "woonplaats",
                table: "ParticulierHuurders");
        }
    }
}

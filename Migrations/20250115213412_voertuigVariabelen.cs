using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class voertuigVariabelen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AantalDeuren",
                table: "Voertuigen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AantalSlaapplekken",
                table: "Voertuigen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Afbeelding",
                table: "Voertuigen",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AantalDeuren",
                table: "Voertuigen");

            migrationBuilder.DropColumn(
                name: "AantalSlaapplekken",
                table: "Voertuigen");

            migrationBuilder.DropColumn(
                name: "Afbeelding",
                table: "Voertuigen");
        }
    }
}

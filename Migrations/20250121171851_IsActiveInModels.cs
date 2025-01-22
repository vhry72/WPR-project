using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class IsActiveInModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ZakelijkHuurders",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WagenparkBeheerders",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Voertuigen",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ParticulierHuurders",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FrontofficeMedewerkers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BedrijfsMedewerkers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BackofficeMedewerkers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Abonnementen",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ZakelijkHuurders");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WagenparkBeheerders");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Voertuigen");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ParticulierHuurders");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FrontofficeMedewerkers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BedrijfsMedewerkers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BackofficeMedewerkers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Abonnementen");
        }
    }
}

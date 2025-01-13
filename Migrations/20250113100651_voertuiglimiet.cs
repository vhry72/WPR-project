using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class voertuiglimiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoortOnderhoud",
                table: "Schademeldingen");

            migrationBuilder.AddColumn<int>(
                name: "voertuiglimiet",
                table: "WagenparkBeheerders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Onderhoud",
                table: "Schademeldingen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reparatie",
                table: "Schademeldingen",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "voertuiglimiet",
                table: "WagenparkBeheerders");

            migrationBuilder.DropColumn(
                name: "Onderhoud",
                table: "Schademeldingen");

            migrationBuilder.DropColumn(
                name: "Reparatie",
                table: "Schademeldingen");

            migrationBuilder.AddColumn<int>(
                name: "SoortOnderhoud",
                table: "Schademeldingen",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

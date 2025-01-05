using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class redenHuurverzoek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BedrijfsMedewerkers_WagenparkBeheerders_WagenparkBeheerderbeheerderId",
                table: "BedrijfsMedewerkers");

            migrationBuilder.AddColumn<string>(
                name: "Reden",
                table: "Huurverzoeken",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WagenparkBeheerderbeheerderId",
                table: "BedrijfsMedewerkers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BedrijfsMedewerkers_WagenparkBeheerders_WagenparkBeheerderbeheerderId",
                table: "BedrijfsMedewerkers",
                column: "WagenparkBeheerderbeheerderId",
                principalTable: "WagenparkBeheerders",
                principalColumn: "beheerderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BedrijfsMedewerkers_WagenparkBeheerders_WagenparkBeheerderbeheerderId",
                table: "BedrijfsMedewerkers");

            migrationBuilder.DropColumn(
                name: "Reden",
                table: "Huurverzoeken");

            migrationBuilder.AlterColumn<Guid>(
                name: "WagenparkBeheerderbeheerderId",
                table: "BedrijfsMedewerkers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_BedrijfsMedewerkers_WagenparkBeheerders_WagenparkBeheerderbeheerderId",
                table: "BedrijfsMedewerkers",
                column: "WagenparkBeheerderbeheerderId",
                principalTable: "WagenparkBeheerders",
                principalColumn: "beheerderId");
        }
    }
}

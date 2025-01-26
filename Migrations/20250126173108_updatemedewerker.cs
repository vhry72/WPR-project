using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class updatemedewerker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BedrijfsMedewerkers_WagenparkBeheerders_beheerderId",
                table: "BedrijfsMedewerkers");

            migrationBuilder.DropForeignKey(
                name: "FK_BedrijfsMedewerkers_ZakelijkHuurders_zakelijkeId",
                table: "BedrijfsMedewerkers");

            migrationBuilder.DropIndex(
                name: "IX_BedrijfsMedewerkers_beheerderId",
                table: "BedrijfsMedewerkers");

            migrationBuilder.DropIndex(
                name: "IX_BedrijfsMedewerkers_zakelijkeId",
                table: "BedrijfsMedewerkers");

            migrationBuilder.AddColumn<Guid>(
                name: "ZakelijkHuurderzakelijkeId",
                table: "BedrijfsMedewerkers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BedrijfsMedewerkers_ZakelijkHuurderzakelijkeId",
                table: "BedrijfsMedewerkers",
                column: "ZakelijkHuurderzakelijkeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BedrijfsMedewerkers_ZakelijkHuurders_ZakelijkHuurderzakelijkeId",
                table: "BedrijfsMedewerkers",
                column: "ZakelijkHuurderzakelijkeId",
                principalTable: "ZakelijkHuurders",
                principalColumn: "zakelijkeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BedrijfsMedewerkers_ZakelijkHuurders_ZakelijkHuurderzakelijkeId",
                table: "BedrijfsMedewerkers");

            migrationBuilder.DropIndex(
                name: "IX_BedrijfsMedewerkers_ZakelijkHuurderzakelijkeId",
                table: "BedrijfsMedewerkers");

            migrationBuilder.DropColumn(
                name: "ZakelijkHuurderzakelijkeId",
                table: "BedrijfsMedewerkers");

            migrationBuilder.CreateIndex(
                name: "IX_BedrijfsMedewerkers_beheerderId",
                table: "BedrijfsMedewerkers",
                column: "beheerderId");

            migrationBuilder.CreateIndex(
                name: "IX_BedrijfsMedewerkers_zakelijkeId",
                table: "BedrijfsMedewerkers",
                column: "zakelijkeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BedrijfsMedewerkers_WagenparkBeheerders_beheerderId",
                table: "BedrijfsMedewerkers",
                column: "beheerderId",
                principalTable: "WagenparkBeheerders",
                principalColumn: "beheerderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BedrijfsMedewerkers_ZakelijkHuurders_zakelijkeId",
                table: "BedrijfsMedewerkers",
                column: "zakelijkeId",
                principalTable: "ZakelijkHuurders",
                principalColumn: "zakelijkeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

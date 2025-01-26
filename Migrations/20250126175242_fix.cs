using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropIndex(
                name: "IX_BedrijfsMedewerkers_ZakelijkHuurderzakelijkeId",
                table: "BedrijfsMedewerkers");

            migrationBuilder.DropColumn(
                name: "ZakelijkHuurderzakelijkeId",
                table: "BedrijfsMedewerkers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ZakelijkHuurderzakelijkeId",
                table: "BedrijfsMedewerkers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BedrijfsMedewerkers_ZakelijkHuurderzakelijkeId",
                table: "BedrijfsMedewerkers",
                column: "ZakelijkHuurderzakelijkeId");

        }
    }
}

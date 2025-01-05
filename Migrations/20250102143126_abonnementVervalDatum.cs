using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class abonnementVervalDatum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "vervaldatum",
                table: "Abonnementen",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vervaldatum",
                table: "Abonnementen");
        }
    }
}

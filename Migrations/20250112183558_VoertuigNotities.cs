using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class VoertuigNotities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "updateDatum",
                table: "Abonnementen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VoertuigNotities",
                columns: table => new
                {
                    NotitieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    voertuigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notitie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotitieDatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoertuigNotities", x => x.NotitieId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoertuigNotities");

            migrationBuilder.DropColumn(
                name: "updateDatum",
                table: "Abonnementen");
        }
    }
}

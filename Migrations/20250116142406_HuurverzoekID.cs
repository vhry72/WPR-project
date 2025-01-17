using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    public partial class HuurverzoekID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the table with the necessary columns and primary key
            migrationBuilder.CreateTable(
                name: "Huurverzoeken",
                columns: table => new
                {
                    HuurVerzoekId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HuurderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    beginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    approved = table.Column<bool>(type: "bit", nullable: false),
                    isBevestigd = table.Column<bool>(type: "bit", nullable: false),
                    Reden = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoertuigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Huurverzoeken", x => x.HuurVerzoekId);
                    table.ForeignKey(
                        name: "FK_Huurverzoeken_Voertuigen_VoertuigId",
                        column: x => x.VoertuigId,
                        principalTable: "Voertuigen",
                        principalColumn: "voertuigId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create an index on the foreign key column if necessary
            migrationBuilder.CreateIndex(
                name: "IX_Huurverzoeken_VoertuigId",
                table: "Huurverzoeken",
                column: "VoertuigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the table if rolling back
            migrationBuilder.DropTable(
                name: "Huurverzoeken");
        }
    }
}

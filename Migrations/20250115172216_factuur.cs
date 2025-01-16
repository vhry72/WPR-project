using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class factuur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facturen",
                columns: table => new
                {
                    FactuurId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BeheerderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AbonnementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FactuurPDF = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FactuurDatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturen", x => x.FactuurId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facturen");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonnementen",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    abonnementNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    term = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    beginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnementen", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Huurverzoeken",
                columns: table => new
                {
                    HuurderID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    voertuigId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    beginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    approved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Huurverzoeken", x => x.HuurderID);
                });

            migrationBuilder.CreateTable(
                name: "Medewerkers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medewerkers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParticulierHuurders",
                columns: table => new
                {
                    gebruikerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticulierHuurders", x => x.gebruikerId);
                });

            migrationBuilder.CreateTable(
                name: "Voertuigen",
                columns: table => new
                {
                    voertuigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    merk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prijsPerDag = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    voertuigType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voertuigen", x => x.voertuigId);
                });

            migrationBuilder.CreateTable(
                name: "WagenParkBeheerders",
                columns: table => new
                {
                    beheerderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    beheerderNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagenParkBeheerders", x => x.beheerderId);
                });

            migrationBuilder.CreateTable(
                name: "ZakelijkHuurders",
                columns: table => new
                {
                    zakelijkeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telNummer = table.Column<int>(type: "int", nullable: false),
                    bedrijfsNaam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZakelijkHuurders", x => x.zakelijkeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abonnementen");

            migrationBuilder.DropTable(
                name: "Huurverzoeken");

            migrationBuilder.DropTable(
                name: "Medewerkers");

            migrationBuilder.DropTable(
                name: "ParticulierHuurders");

            migrationBuilder.DropTable(
                name: "Voertuigen");

            migrationBuilder.DropTable(
                name: "WagenParkBeheerders");

            migrationBuilder.DropTable(
                name: "ZakelijkHuurders");
        }
    }
}

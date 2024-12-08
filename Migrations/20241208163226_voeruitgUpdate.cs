using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class voeruitgUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonnementen",
                columns: table => new
                {
                    AbonnementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    abonnementNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    term = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    beginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnementen", x => x.AbonnementId);
                });

            migrationBuilder.CreateTable(
                name: "BedrijfsMedewerkers",
                columns: table => new
                {
                    BedrijfsMedewerkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    medewerkerNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    medewerkerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedrijfsMedewerkers", x => x.BedrijfsMedewerkId);
                });

            migrationBuilder.CreateTable(
                name: "Bedrijven",
                columns: table => new
                {
                    BedrijfId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bedrijfsNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bedrijfsAdres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KvkNummer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bedrijven", x => x.BedrijfId);
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
                    medewerkerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    medewerkerNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    medewerkerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    medewerkerRol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medewerkers", x => x.medewerkerId);
                });

            migrationBuilder.CreateTable(
                name: "ParticulierHuurders",
                columns: table => new
                {
                    particulierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    particulierEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    particulierNaam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmailBevestigingToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsEmailBevestigd = table.Column<bool>(type: "bit", nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    adress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    woonplaats = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    telefoonnummer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticulierHuurders", x => x.particulierId);
                });

            migrationBuilder.CreateTable(
                name: "Voertuigen",
                columns: table => new
                {
                    voertuigId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    merk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prijsPerDag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    voertuigType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voertuigen", x => x.voertuigId);
                });

            migrationBuilder.CreateTable(
                name: "VoertuigStatussen",
                columns: table => new
                {
                    VoertuigStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    verhuurd = table.Column<bool>(type: "bit", nullable: false),
                    schade = table.Column<bool>(type: "bit", nullable: false),
                    onderhoud = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoertuigStatussen", x => x.VoertuigStatusId);
                });

            migrationBuilder.CreateTable(
                name: "WagenparkBeheerders",
                columns: table => new
                {
                    beheerderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    beheerderNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telNummer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagenparkBeheerders", x => x.beheerderId);
                });

            migrationBuilder.CreateTable(
                name: "ZakelijkHuurders",
                columns: table => new
                {
                    zakelijkeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    adres = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KVKNummer = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailBevestigingToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsEmailBevestigd = table.Column<bool>(type: "bit", nullable: false),
                    telNummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bedrijfsNaam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedewerkersEmails = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZakelijkHuurders", x => x.zakelijkeId);
                });

            migrationBuilder.CreateTable(
                name: "BackofficeMedewerkers",
                columns: table => new
                {
                    medewerkerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackofficeMedewerkers", x => x.medewerkerId);
                    table.ForeignKey(
                        name: "FK_BackofficeMedewerkers_Medewerkers_medewerkerId",
                        column: x => x.medewerkerId,
                        principalTable: "Medewerkers",
                        principalColumn: "medewerkerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FrontofficeMedewerkers",
                columns: table => new
                {
                    medewerkerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrontofficeMedewerkers", x => x.medewerkerId);
                    table.ForeignKey(
                        name: "FK_FrontofficeMedewerkers_Medewerkers_medewerkerId",
                        column: x => x.medewerkerId,
                        principalTable: "Medewerkers",
                        principalColumn: "medewerkerId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abonnementen");

            migrationBuilder.DropTable(
                name: "BackofficeMedewerkers");

            migrationBuilder.DropTable(
                name: "BedrijfsMedewerkers");

            migrationBuilder.DropTable(
                name: "Bedrijven");

            migrationBuilder.DropTable(
                name: "FrontofficeMedewerkers");

            migrationBuilder.DropTable(
                name: "Huurverzoeken");

            migrationBuilder.DropTable(
                name: "ParticulierHuurders");

            migrationBuilder.DropTable(
                name: "Voertuigen");

            migrationBuilder.DropTable(
                name: "VoertuigStatussen");

            migrationBuilder.DropTable(
                name: "WagenparkBeheerders");

            migrationBuilder.DropTable(
                name: "ZakelijkHuurders");

            migrationBuilder.DropTable(
                name: "Medewerkers");
        }
    }
}

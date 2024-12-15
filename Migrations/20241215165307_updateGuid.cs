using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class updateGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abonnementen",
                columns: table => new
                {
                    AbonnementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kosten = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AbonnementType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnementen", x => x.AbonnementId);
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
                    HuurderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    voertuigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    particulierNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    voertuigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    merk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kleur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prijsPerDag = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    voertuigType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bouwjaar = table.Column<int>(type: "int", nullable: false),
                    kenteken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    eindDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    voertuigBeschikbaar = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voertuigen", x => x.voertuigId);
                });

            migrationBuilder.CreateTable(
                name: "ZakelijkHuurders",
                columns: table => new
                {
                    zakelijkeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    adres = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KVKNummer = table.Column<int>(type: "int", nullable: false),
                    bedrijsEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailBevestigingToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsEmailBevestigd = table.Column<bool>(type: "bit", nullable: false),
                    telNummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bedrijfsNaam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZakelijkHuurders", x => x.zakelijkeId);
                });

            migrationBuilder.CreateTable(
                name: "WagenparkBeheerders",
                columns: table => new
                {
                    beheerderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    beheerderNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    bedrijfsEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KVKNummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefoonNummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbonnementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    updateDatumAbonnement = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AbonnementType = table.Column<int>(type: "int", nullable: true),
                    PrepaidSaldo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagenparkBeheerders", x => x.beheerderId);
                    table.ForeignKey(
                        name: "FK_WagenparkBeheerders_Abonnementen_AbonnementId",
                        column: x => x.AbonnementId,
                        principalTable: "Abonnementen",
                        principalColumn: "AbonnementId");
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

            migrationBuilder.CreateTable(
                name: "VoertuigStatussen",
                columns: table => new
                {
                    VoertuigStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    verhuurd = table.Column<bool>(type: "bit", nullable: false),
                    schade = table.Column<bool>(type: "bit", nullable: false),
                    onderhoud = table.Column<bool>(type: "bit", nullable: false),
                    voertuigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoertuigStatussen", x => x.VoertuigStatusId);
                    table.ForeignKey(
                        name: "FK_VoertuigStatussen_Voertuigen_voertuigId",
                        column: x => x.voertuigId,
                        principalTable: "Voertuigen",
                        principalColumn: "voertuigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BedrijfsMedewerkers",
                columns: table => new
                {
                    bedrijfsMedewerkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    medewerkerNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    medewerkerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    zakelijkeHuurderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AbonnementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WagenparkBeheerderbeheerderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedrijfsMedewerkers", x => x.bedrijfsMedewerkerId);
                    table.ForeignKey(
                        name: "FK_BedrijfsMedewerkers_Abonnementen_AbonnementId",
                        column: x => x.AbonnementId,
                        principalTable: "Abonnementen",
                        principalColumn: "AbonnementId");
                    table.ForeignKey(
                        name: "FK_BedrijfsMedewerkers_WagenparkBeheerders_WagenparkBeheerderbeheerderId",
                        column: x => x.WagenparkBeheerderbeheerderId,
                        principalTable: "WagenparkBeheerders",
                        principalColumn: "beheerderId");
                    table.ForeignKey(
                        name: "FK_BedrijfsMedewerkers_ZakelijkHuurders_zakelijkeHuurderId",
                        column: x => x.zakelijkeHuurderId,
                        principalTable: "ZakelijkHuurders",
                        principalColumn: "zakelijkeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BedrijfsMedewerkers_AbonnementId",
                table: "BedrijfsMedewerkers",
                column: "AbonnementId");

            migrationBuilder.CreateIndex(
                name: "IX_BedrijfsMedewerkers_WagenparkBeheerderbeheerderId",
                table: "BedrijfsMedewerkers",
                column: "WagenparkBeheerderbeheerderId");

            migrationBuilder.CreateIndex(
                name: "IX_BedrijfsMedewerkers_zakelijkeHuurderId",
                table: "BedrijfsMedewerkers",
                column: "zakelijkeHuurderId");

            migrationBuilder.CreateIndex(
                name: "IX_VoertuigStatussen_voertuigId",
                table: "VoertuigStatussen",
                column: "voertuigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WagenparkBeheerders_AbonnementId",
                table: "WagenparkBeheerders",
                column: "AbonnementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "VoertuigStatussen");

            migrationBuilder.DropTable(
                name: "WagenparkBeheerders");

            migrationBuilder.DropTable(
                name: "ZakelijkHuurders");

            migrationBuilder.DropTable(
                name: "Medewerkers");

            migrationBuilder.DropTable(
                name: "Voertuigen");

            migrationBuilder.DropTable(
                name: "Abonnementen");
        }
    }
}

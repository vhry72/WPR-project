using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class database : Migration
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
                    vervaldatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kosten = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AbonnementType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnementen", x => x.AbonnementId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BackofficeMedewerkers",
                columns: table => new
                {
                    BackofficeMedewerkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    medewerkerNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    medewerkerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AspNetUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackofficeMedewerkers", x => x.BackofficeMedewerkerId);
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
                name: "FrontofficeMedewerkers",
                columns: table => new
                {
                    FrontofficeMedewerkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    medewerkerNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    medewerkerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AspNetUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrontofficeMedewerkers", x => x.FrontofficeMedewerkerId);
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
                    bedrijfsEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailBevestigingToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEmailBevestigd = table.Column<bool>(type: "bit", nullable: false),
                    telNummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bedrijfsNaam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AspNetUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    PrepaidSaldo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AspNetUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticulierHuurders",
                columns: table => new
                {
                    particulierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    particulierEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    particulierNaam = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailBevestigingToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEmailBevestigd = table.Column<bool>(type: "bit", nullable: false),
                    wachtwoord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    adress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    woonplaats = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    telefoonnummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AspNetUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticulierHuurders", x => x.particulierId);
                    table.ForeignKey(
                        name: "FK_ParticulierHuurders_AspNetUsers_AspNetUserId",
                        column: x => x.AspNetUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Huurverzoeken",
                columns: table => new
                {
                    HuurderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    beginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    approved = table.Column<bool>(type: "bit", nullable: false),
                    isBevestigd = table.Column<bool>(type: "bit", nullable: false),
                    VoertuigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reden = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Huurverzoeken", x => x.HuurderID);
                    table.ForeignKey(
                        name: "FK_Huurverzoeken_Voertuigen_VoertuigId",
                        column: x => x.VoertuigId,
                        principalTable: "Voertuigen",
                        principalColumn: "voertuigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schademeldingen",
                columns: table => new
                {
                    SchademeldingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Beschrijving = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opmerkingen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoertuigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schademeldingen", x => x.SchademeldingId);
                    table.ForeignKey(
                        name: "FK_Schademeldingen_Voertuigen_VoertuigId",
                        column: x => x.VoertuigId,
                        principalTable: "Voertuigen",
                        principalColumn: "voertuigId",
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
                    WagenparkBeheerderbeheerderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AspNetUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbonnementId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                        principalColumn: "beheerderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BedrijfsMedewerkers_ZakelijkHuurders_zakelijkeHuurderId",
                        column: x => x.zakelijkeHuurderId,
                        principalTable: "ZakelijkHuurders",
                        principalColumn: "zakelijkeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BedrijfsMedewerkers_AbonnementId",
                table: "BedrijfsMedewerkers",
                column: "AbonnementId");

            migrationBuilder.CreateIndex(
                name: "IX_BedrijfsMedewerkers_WagenparkBeheerderbeheerderId",
                table: "BedrijfsMedewerkers",
                column: "beheerderId");

            migrationBuilder.CreateIndex(
                name: "IX_BedrijfsMedewerkers_zakelijkeHuurderId",
                table: "BedrijfsMedewerkers",
                column: "zakelijkeHuurderId");

            migrationBuilder.CreateIndex(
                name: "IX_Huurverzoeken_VoertuigId",
                table: "Huurverzoeken",
                column: "VoertuigId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticulierHuurders_AspNetUserId",
                table: "ParticulierHuurders",
                column: "AspNetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Schademeldingen_VoertuigId",
                table: "Schademeldingen",
                column: "VoertuigId");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

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
                name: "Schademeldingen");

            migrationBuilder.DropTable(
                name: "VoertuigStatussen");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "WagenparkBeheerders");

            migrationBuilder.DropTable(
                name: "ZakelijkHuurders");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Voertuigen");

            migrationBuilder.DropTable(
                name: "Abonnementen");
        }
    }
}

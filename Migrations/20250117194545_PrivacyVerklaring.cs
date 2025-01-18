using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class PrivacyVerklaring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AbonnementId",
                table: "BedrijfsMedewerkers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "PrivacyVerklaringen",
                columns: table => new
                {
                    VerklaringId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedewerkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Verklaring = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivacyVerklaringen", x => x.VerklaringId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivacyVerklaringen");

            migrationBuilder.AlterColumn<Guid>(
                name: "AbonnementId",
                table: "BedrijfsMedewerkers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}

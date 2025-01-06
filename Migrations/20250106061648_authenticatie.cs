using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class authenticatie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmailBevestigingToken",
                table: "WagenparkBeheerders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailBevestigd",
                table: "WagenparkBeheerders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "EmailBevestigingToken",
                table: "FrontofficeMedewerkers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailBevestigd",
                table: "FrontofficeMedewerkers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "EmailBevestigingToken",
                table: "BedrijfsMedewerkers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailBevestigd",
                table: "BedrijfsMedewerkers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "EmailBevestigingToken",
                table: "BackofficeMedewerkers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailBevestigd",
                table: "BackofficeMedewerkers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailBevestigingToken",
                table: "WagenparkBeheerders");

            migrationBuilder.DropColumn(
                name: "IsEmailBevestigd",
                table: "WagenparkBeheerders");

            migrationBuilder.DropColumn(
                name: "EmailBevestigingToken",
                table: "FrontofficeMedewerkers");

            migrationBuilder.DropColumn(
                name: "IsEmailBevestigd",
                table: "FrontofficeMedewerkers");

            migrationBuilder.DropColumn(
                name: "EmailBevestigingToken",
                table: "BedrijfsMedewerkers");

            migrationBuilder.DropColumn(
                name: "IsEmailBevestigd",
                table: "BedrijfsMedewerkers");

            migrationBuilder.DropColumn(
                name: "EmailBevestigingToken",
                table: "BackofficeMedewerkers");

            migrationBuilder.DropColumn(
                name: "IsEmailBevestigd",
                table: "BackofficeMedewerkers");
        }
    }
}

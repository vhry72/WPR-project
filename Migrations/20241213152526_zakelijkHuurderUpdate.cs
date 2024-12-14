using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WPR_project.Migrations
{
    /// <inheritdoc />
    public partial class zakelijkHuurderUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZakelijkHuurders_Abonnementen_AbonnementId",
                table: "ZakelijkHuurders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateDatumAbonnement",
                table: "ZakelijkHuurders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrepaidSaldo",
                table: "ZakelijkHuurders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "AbonnementType",
                table: "ZakelijkHuurders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "AbonnementId",
                table: "ZakelijkHuurders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ZakelijkHuurders_Abonnementen_AbonnementId",
                table: "ZakelijkHuurders",
                column: "AbonnementId",
                principalTable: "Abonnementen",
                principalColumn: "AbonnementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ZakelijkHuurders_Abonnementen_AbonnementId",
                table: "ZakelijkHuurders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateDatumAbonnement",
                table: "ZakelijkHuurders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PrepaidSaldo",
                table: "ZakelijkHuurders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AbonnementType",
                table: "ZakelijkHuurders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AbonnementId",
                table: "ZakelijkHuurders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ZakelijkHuurders_Abonnementen_AbonnementId",
                table: "ZakelijkHuurders",
                column: "AbonnementId",
                principalTable: "Abonnementen",
                principalColumn: "AbonnementId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

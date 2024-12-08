﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WPR_project.Data;

#nullable disable

namespace WPR_project.Migrations
{
    [DbContext(typeof(GegevensContext))]
    partial class GegevensContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WPR_project.Models.Abonnement", b =>
                {
                    b.Property<int>("AbonnementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AbonnementId"));

                    b.Property<string>("abonnementNaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("beginDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("endDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("price")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("term")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AbonnementId");

                    b.ToTable("Abonnementen");
                });

            modelBuilder.Entity("WPR_project.Models.Bedrijf", b =>
                {
                    b.Property<int>("BedrijfId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BedrijfId"));

                    b.Property<int>("KvkNummer")
                        .HasColumnType("int");

                    b.Property<string>("bedrijfsAdres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bedrijfsNaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BedrijfId");

                    b.ToTable("Bedrijven");
                });

            modelBuilder.Entity("WPR_project.Models.BedrijfsMedewerkers", b =>
                {
                    b.Property<int>("BedrijfsMedewerkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BedrijfsMedewerkId"));

                    b.Property<string>("medewerkerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("medewerkerNaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BedrijfsMedewerkId");

                    b.ToTable("BedrijfsMedewerkers");
                });

            modelBuilder.Entity("WPR_project.Models.Huurverzoek", b =>
                {
                    b.Property<string>("HuurderID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("approved")
                        .HasColumnType("bit");

                    b.Property<DateTime>("beginDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("endDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("voertuigId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HuurderID");

                    b.ToTable("Huurverzoeken");
                });

            modelBuilder.Entity("WPR_project.Models.Medewerker", b =>
                {
                    b.Property<int>("medewerkerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("medewerkerId"));

                    b.Property<string>("medewerkerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("medewerkerNaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("medewerkerRol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("medewerkerId");

                    b.ToTable("Medewerkers", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("WPR_project.Models.ParticulierHuurder", b =>
                {
                    b.Property<int>("particulierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("particulierId"));

                    b.Property<string>("EmailBevestigingToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailBevestigd")
                        .HasColumnType("bit");

                    b.Property<string>("adress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("particulierEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("particulierNaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("postcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("telefoonnummer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("wachtwoord")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("woonplaats")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("particulierId");

                    b.ToTable("ParticulierHuurders");
                });

            modelBuilder.Entity("WPR_project.Models.Voertuig", b =>
                {
                    b.Property<int>("voertuigId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("voertuigId"));

                    b.Property<string>("merk")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("prijsPerDag")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("voertuigType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("voertuigId");

                    b.ToTable("Voertuigen");
                });

            modelBuilder.Entity("WPR_project.Models.VoertuigStatus", b =>
                {
                    b.Property<int>("VoertuigStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VoertuigStatusId"));

                    b.Property<bool>("onderhoud")
                        .HasColumnType("bit");

                    b.Property<bool>("schade")
                        .HasColumnType("bit");

                    b.Property<bool>("verhuurd")
                        .HasColumnType("bit");

                    b.HasKey("VoertuigStatusId");

                    b.ToTable("VoertuigStatussen");
                });

            modelBuilder.Entity("WPR_project.Models.WagenparkBeheerder", b =>
                {
                    b.Property<int>("beheerderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("beheerderId"));

                    b.Property<string>("beheerderNaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("telNummer")
                        .HasColumnType("int");

                    b.HasKey("beheerderId");

                    b.ToTable("WagenparkBeheerders");
                });

            modelBuilder.Entity("WPR_project.Models.ZakelijkHuurder", b =>
                {
                    b.Property<int>("zakelijkeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("zakelijkeId"));

                    b.Property<string>("EmailBevestigingToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailBevestigd")
                        .HasColumnType("bit");

                    b.Property<int>("KVKNummer")
                        .HasColumnType("int");

                    b.PrimitiveCollection<string>("MedewerkersEmails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("adres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bedrijfsNaam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("telNummer")
                        .HasColumnType("int");

                    b.Property<string>("wachtwoord")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("zakelijkeId");

                    b.ToTable("ZakelijkHuurders");
                });

            modelBuilder.Entity("WPR_project.Models.BackofficeMedewerker", b =>
                {
                    b.HasBaseType("WPR_project.Models.Medewerker");

                    b.ToTable("BackofficeMedewerkers", (string)null);
                });

            modelBuilder.Entity("WPR_project.Models.FrontofficeMedewerker", b =>
                {
                    b.HasBaseType("WPR_project.Models.Medewerker");

                    b.ToTable("FrontofficeMedewerkers", (string)null);
                });

            modelBuilder.Entity("WPR_project.Models.BackofficeMedewerker", b =>
                {
                    b.HasOne("WPR_project.Models.Medewerker", null)
                        .WithOne()
                        .HasForeignKey("WPR_project.Models.BackofficeMedewerker", "medewerkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WPR_project.Models.FrontofficeMedewerker", b =>
                {
                    b.HasOne("WPR_project.Models.Medewerker", null)
                        .WithOne()
                        .HasForeignKey("WPR_project.Models.FrontofficeMedewerker", "medewerkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

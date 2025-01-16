﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WPR_project.Models;

namespace WPR_project.Data
{
    public class GegevensContext : IdentityDbContext<IdentityUser>
    {
        public GegevensContext(DbContextOptions<GegevensContext> options) : base(options)
        {
        }

        // DbSet-definities
        public DbSet<ParticulierHuurder> ParticulierHuurders { get; set; }
        public DbSet<ZakelijkHuurder> ZakelijkHuurders { get; set; }
        public DbSet<Voertuig> Voertuigen { get; set; }
        public DbSet<Abonnement> Abonnementen { get; set; }
        public DbSet<Huurverzoek> Huurverzoeken { get; set; }
        public DbSet<WagenparkBeheerder> WagenparkBeheerders { get; set; }
        public DbSet<BackofficeMedewerker> BackofficeMedewerkers { get; set; }
        public DbSet<Bedrijf> Bedrijven { get; set; }
        public DbSet<BedrijfsMedewerkers> BedrijfsMedewerkers { get; set; }
        public DbSet<FrontofficeMedewerker> FrontofficeMedewerkers { get; set; }
        public DbSet<Schademelding> Schademeldingen { get; set; }
        public DbSet<VoertuigStatus> VoertuigStatussen { get; set; }

        public DbSet<VoertuigNotities> VoertuigNotities { get; set; }
        public DbSet<Factuur> Facturen { get; set; }

        // Configuratie van de modellen en relaties
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Configuratie voor Medewerkers
            modelBuilder.Entity<BackofficeMedewerker>()
                .HasKey(b => b.BackofficeMedewerkerId);


            modelBuilder.Entity<FrontofficeMedewerker>()
                .HasKey(b => b.FrontofficeMedewerkerId);

            // Voertuig Configuratie
            modelBuilder.Entity<Voertuig>()
             .HasKey(v => v.voertuigId);

            modelBuilder.Entity<Factuur>()
                .HasKey(f => f.FactuurId);

            // Abonnement Configuratie
            modelBuilder.Entity<Abonnement>()
                .HasKey(a => a.AbonnementId);

            modelBuilder.Entity<VoertuigNotities>()
                .HasKey(v => v.NotitieId);
                

            // Huurverzoek Configuratie
            modelBuilder.Entity<Huurverzoek>()
                .HasKey(h => h.HuurderID);

            // WagenParkBeheerder Configuratie
            modelBuilder.Entity<WagenparkBeheerder>()
                .HasKey(w => w.beheerderId);


            // Bedrijf Configuratie
            modelBuilder.Entity<Bedrijf>()
                .HasKey(b => b.BedrijfId);

            // BedrijfsMedewerker Configuratie
            modelBuilder.Entity<BedrijfsMedewerkers>()
                .HasKey(b => b.bedrijfsMedewerkerId);

            modelBuilder.Entity<VoertuigStatus>()
            .HasOne(vs => vs.voertuig)
            .WithMany() // Geen navigatie-eigenschap in Voertuig
            .HasForeignKey(vs => vs.voertuigId)
            .OnDelete(DeleteBehavior.Cascade);

            // ZakelijkHuurder Configuratie
            modelBuilder.Entity<ZakelijkHuurder>()
                .HasKey(z => z.zakelijkeId);

            // ParticulierHuurder Configuratie
            modelBuilder.Entity<ParticulierHuurder>()
                .HasKey(p => p.particulierId);

            modelBuilder.Entity<ParticulierHuurder>(entity =>
            {
                entity.HasOne<IdentityUser>()
                      .WithMany()
                      .HasForeignKey(p => p.AspNetUserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //SchadeMelding Configuratie
            modelBuilder.Entity<Schademelding>()
                .HasOne(s => s.Voertuig)
                .WithMany(v => v.Schademeldingen)
                .HasForeignKey(s => s.VoertuigId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-3JG6Q2V;Database=HuurderRegistratie;Trusted_Connection=True;");
            }
        }
    }
}

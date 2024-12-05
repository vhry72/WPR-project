using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WPR_project.Models;

namespace WPR_project.Data
{
    public class GegevensContext : DbContext
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
        public DbSet<WagenParkBeheerder> WagenParkBeheerders { get; set; }
        public DbSet<BackofficeMedewerker> BackofficeMedewerkers { get; set; }
        public DbSet<Bedrijf> Bedrijven { get; set; }
        public DbSet<BedrijfsMedewerkers> BedrijfsMedewerkers { get; set; }
        public DbSet<FrontofficeMedewerker> FrontofficeMedewerkers { get; set; }
        
        public DbSet<VoertuigStatus> VoertuigStatussen { get; set; }

        // Configuratie van de modellen en relaties
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Huurder>().ToTable("Huurders");
            modelBuilder.Entity<ParticulierHuurder>().ToTable("ParticulierHuurders");
            modelBuilder.Entity<ZakelijkHuurder>().ToTable("ZakelijkHuurders");

            modelBuilder.Entity<Huurder>()
                .HasKey(v => v.HuurderId);

            modelBuilder.Entity<ParticulierHuurder>()
                .HasBaseType<Huurder>();


            modelBuilder.Entity<ZakelijkHuurder>()
                .HasBaseType<Huurder>();
                

            // TPT Configuratie voor Medewerkers
            modelBuilder.Entity<Medewerker>().ToTable("Medewerkers");
            modelBuilder.Entity<BackofficeMedewerker>().ToTable("BackofficeMedewerkers");
            modelBuilder.Entity<FrontofficeMedewerker>().ToTable("FrontofficeMedewerkers");

            modelBuilder.Entity<Medewerker>()
                .HasKey(v => v.medewerkerId);

            modelBuilder.Entity<BackofficeMedewerker>()
                .HasBaseType<Medewerker>();


            modelBuilder.Entity<FrontofficeMedewerker>()
                .HasBaseType<Medewerker>();
                




            // Voertuig Configuratie
            modelBuilder.Entity<Voertuig>()
                .HasKey(v => v.voertuigId);

            // Abonnement Configuratie
            modelBuilder.Entity<Abonnement>()
                .HasKey(a => a.AbonnementId);

            // Huurverzoek Configuratie
            modelBuilder.Entity<Huurverzoek>()
                .HasKey(h => h.HuurderID);

            // WagenParkBeheerder Configuratie
            modelBuilder.Entity<WagenParkBeheerder>()
                .HasKey(w => w.beheerderId);


            // Bedrijf Configuratie
            modelBuilder.Entity<Bedrijf>()
                .HasKey(b => b.BedrijfId);

            // BedrijfsMedewerker Configuratie
            modelBuilder.Entity<BedrijfsMedewerkers>()
                .HasKey(b => b.BedrijfsMedewerkId);

            // VoertuigStatus Configuratie
            modelBuilder.Entity<VoertuigStatus>()
                .HasKey(vs => vs.VoertuigStatusId);

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

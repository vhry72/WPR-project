using Microsoft.EntityFrameworkCore;
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
        public DbSet<Medewerker> Medewerkers { get; set; }
        public DbSet<WagenParkBeheerder> WagenParkBeheerders { get; set; }

        // Configuratie van de modellen en relaties
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ParticulierHuurder Configuratie
            modelBuilder.Entity<ParticulierHuurder>(entity =>
            {
                entity.ToTable("ParticulierHuurders");
                entity.HasKey(e => e.gebruikerId); // Primaire sleutel
                entity.Property(e => e.naam).IsRequired(); // Verplichte kolom
            });

            // ZakelijkHuurder Configuratie
            modelBuilder.Entity<ZakelijkHuurder>(entity =>
            {
                entity.ToTable("ZakelijkHuurders");
                entity.HasKey(e => e.zakelijkeId);
                entity.Property(e => e.bedrijfsNaam).IsRequired();
            });

            // Voertuig Configuratie
            modelBuilder.Entity<Voertuig>(entity =>
            {
                entity.ToTable("Voertuigen");
                entity.HasKey(e => e.voertuigId); // Primaire sleutel
                entity.Property(e => e.merk).IsRequired();
                entity.Property(e => e.model).IsRequired();
                entity.Property(e => e.prijsPerDag).HasColumnType("decimal(18,2)");
            });

            // Huurverzoek Configuratie
            modelBuilder.Entity<Huurverzoek>(entity =>
            {
                entity.ToTable("Huurverzoeken");
                entity.HasKey(e => e.HuurderID);

                // <!!!!!!!!!!!!!!!!!!!!!> hier moeten de id's gecorrigeerd nog worden!

                //// Relatie: Huurverzoek -> Voertuig
                //entity.HasOne(h => h.voertuigId)
                //      .WithMany(v => v.Huurverzoeken)
                //      .HasForeignKey(h => h.VoertuigId);

                //// Relatie: Huurverzoek -> ParticulierHuurder (optioneel)
                //entity.HasOne(h => h.ParticulierHuurder)
                //      .WithMany(p => p.Huurverzoeken)
                //      .HasForeignKey(h => h.ParticulierHuurderId)
                //      .IsRequired(false);

                //// Relatie: Huurverzoek -> ZakelijkHuurder (optioneel)
                //entity.HasOne(h => h.ZakelijkHuurder)
                //      .WithMany(z => z.Huurverzoeken)
                //      .HasForeignKey(h => h.ZakelijkHuurderId)
                //      .IsRequired(false);
            });

            // WagenParkBeheerder Configuratie
            modelBuilder.Entity<WagenParkBeheerder>(entity =>
            {
                entity.ToTable("WagenParkBeheerders");
                entity.HasKey(e => e.beheerderId);
                entity.Property(e => e.beheerderNaam).IsRequired();
            });

            // Eventueel: andere entiteiten configureren
        }

        // Connection string voor SQL Server
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-3JG6Q2V;Database=HuurderRegistratie;Trusted_Connection=True;");
            }
        }
    }
}
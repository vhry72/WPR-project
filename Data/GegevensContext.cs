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
        public DbSet<WagenparkBeheerder> WagenParkBeheerders { get; set; }

        // Configuratie van de modellen en relaties
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ParticulierHuurder Configuratie
            modelBuilder.Entity<ParticulierHuurder>(entity =>
            {
                entity.ToTable("ParticulierHuurders");
                entity.HasKey(e => e.particulierId); // Primaire sleutel
                entity.Property(e => e.particulierNaam).IsRequired(); // Verplichte kolom
            });

            //zakelijkHuurder configuratie

            modelBuilder.Entity<ZakelijkHuurder>(entity =>
            {
                entity.ToTable("ZakelijkHuurders");
                entity.HasKey(e => e.zakelijkeId);
                entity.Property(e => e.bedrijfsNaam).IsRequired();

                // Conversie voor MedewerkersEmails (opgeslagen als komma-gescheiden string)
                entity.Property(e => e.MedewerkersEmails)
                    .HasConversion(
                        v => string.Join(',', v), // Opslaan als string
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() // Lezen als lijst
                    )
                    .HasColumnName("MedewerkersEmails") // Optioneel: expliciete naam in de database
                    .IsRequired(false); // Optioneel: maak dit niet verplicht
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

                // < !!!!!!!!!!!!!!!!!!!!!> hier moeten de id's gecorrigeerd nog worden!

                // Relatie: Huurverzoek -> Voertuig nog invullen
               
            });

            // WagenparkBeheerder Configuratie
            modelBuilder.Entity<WagenparkBeheerder>(entity =>
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

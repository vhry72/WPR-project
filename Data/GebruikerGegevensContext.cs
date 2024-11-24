using Microsoft.EntityFrameworkCore;

namespace WPR_project.Data
{
    public class GebruikerGegevensContext : DbContext
    {
        public GebruikerGegevensContext(DbContextOptions<GebruikerGegevensContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WPR_project.Models.ParticulierHuurder>(entity =>
            {
                entity.ToTable("ParticulierHuurder");
                entity.HasKey(e => e.gebruikerId); // Stel de primaire sleutel in
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-3JG6Q2V;Database=HuurderRegistratie;Trusted_Connection=True;");
        }

        public DbSet<WPR_project.Models.ParticulierHuurder> ParticulierHuurder { get; set; }
       
    }
}

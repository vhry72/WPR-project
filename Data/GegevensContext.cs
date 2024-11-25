using Microsoft.EntityFrameworkCore;

namespace WPR_project.Data
{
    public class GegevensContext : DbContext
    {
        public GegevensContext(DbContextOptions<GegevensContext> options) : base(options)
        {
        }
        public DbSet<WPR_project.Models.ParticulierHuurder> ParticulierHuurders { get; set; }
        public DbSet<WPR_project.Models.ZakelijkHuurder> ZakelijkHuurders { get; set; }
        public DbSet<WPR_project.Models.Voertuig> Voertuigen { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WPR_project.Models.ParticulierHuurder>(entity =>
            {
                entity.ToTable("ParticulierHuurders");
                entity.HasKey(e => e.gebruikerId); // Stel de primaire sleutel in
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-3JG6Q2V;Database=HuurderRegistratie;Trusted_Connection=True;");
        }
       
    }
}

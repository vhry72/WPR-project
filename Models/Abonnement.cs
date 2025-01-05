using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WPR_project.Models;

public class Abonnement
{
    [Key]
    public Guid AbonnementId { get; set; }

    [Required]
    public string Naam { get; set; }

    [Precision(18, 2)]
    public decimal Kosten { get; set; }

    public DateTime vervaldatum { get; set; }
    public AbonnementType AbonnementType { get; set; }

    public ICollection<WagenparkBeheerder> WagenparkBeheerders { get; set; } = new List<WagenparkBeheerder>();

    public ICollection<BedrijfsMedewerkers> Medewerkers { get; set; } = new List<BedrijfsMedewerkers>();
}
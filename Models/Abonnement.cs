using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WPR_project.Models;

public class Abonnement
{
    [Key]
    public Guid AbonnementId { get; set; }

    [Required]
    public string Naam { get; set; }

    public decimal Kosten { get; set; }

    public ICollection<ZakelijkHuurder> ZakelijkeHuurders { get; set; } = new List<ZakelijkHuurder>();

    public ICollection<BedrijfsMedewerkers> Medewerkers { get; set; } = new List<BedrijfsMedewerkers>();
}
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WPR_project.Models;

public class Abonnement
{
    [Key]
    public Guid AbonnementId { get; set; }

    [Required(ErrorMessage = "Naam is verplicht.")]
    [StringLength(100, ErrorMessage = "Naam mag niet langer zijn dan 100 tekens.")]
    public string Naam { get; set; }

    [Required(ErrorMessage = "Kosten zijn verplicht.")]
    [Range(0, double.MaxValue, ErrorMessage = "Kosten moeten positief zijn.")]
    [Precision(18, 2)]
    public decimal Kosten { get; set; }

    [StringLength(500, ErrorMessage = "Beschrijving mag niet langer zijn dan 500 tekens.")]
    public string Beschrijving { get; set; }

    public List<ZakelijkHuurder> ZakelijkeHuurders { get; set; } = new List<ZakelijkHuurder>();
}
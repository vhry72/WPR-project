using System.ComponentModel.DataAnnotations;
using WPR_project.Models;
using Xunit.Sdk;

public class ZakelijkHuurder
{
    [Key]
    public Guid zakelijkeId { get; set; }

    [Required(ErrorMessage = "Adres is verplicht.")]
    [StringLength(200, ErrorMessage = "Adres mag niet langer zijn dan 200 tekens.")]
    public string adres { get; set; }

    [Required(ErrorMessage = "KVK-nummer is verplicht.")]
    [Range(10000000, 99999999, ErrorMessage = "KVK-nummer moet een 8-cijferig getal zijn.")]
    public int KVKNummer { get; set; }

    [Required(ErrorMessage = "E-mailadres is verplicht.")]
    [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
    public string bedrijsEmail { get; set; }

    public string domein => bedrijsEmail.Split('@')[1]; // Automatisch het domein uit de e-mail halen

    [StringLength(255, ErrorMessage = "E-mail bevestiging token mag niet langer zijn dan 255 tekens.")]
    public string EmailBevestigingToken { get; set; }

    public bool IsEmailBevestigd { get; set; } = false;

    [Required(ErrorMessage = "Telefoonnummer is verplicht.")]
    [RegularExpression(@"^(\+31|0)[1-9]\d{8}$", ErrorMessage = "Telefoonnummer moet een geldig Nederlands telefoonnummer zijn.")]
    public string telNummer { get; set; }

    [Required(ErrorMessage = "Bedrijfsnaam is verplicht.")]
    [StringLength(100, ErrorMessage = "Bedrijfsnaam mag niet langer zijn dan 100 tekens.")]
    public string bedrijfsNaam { get; set; }

    [Required(ErrorMessage = "Wachtwoord is verplicht.")]
    [MinLength(8, ErrorMessage = "Wachtwoord moet minimaal 8 tekens bevatten.")]
    public string wachtwoord { get; set; }

    public Guid AbonnementId { get; set; }
    public Abonnement HuidigAbonnement { get; set; }

    public DateTime updateDatumAbonnement { get; set; }

    public AbonnementType AbonnementType { get; set; }
    public decimal PrepaidSaldo { get; set; } = 0;

    public List<BedrijfsMedewerkers> Medewerkers { get; set; } = new List<BedrijfsMedewerkers>();
}

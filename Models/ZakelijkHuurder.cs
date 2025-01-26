using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
    public string bedrijfsEmail { get; set; }

    public string domein => bedrijfsEmail.Split('@')[1]; // Automatisch het domein uit de e-mail halen

    
    public Guid EmailBevestigingToken { get; set; }

    public bool IsEmailBevestigd { get; set; } = false;

    [Required(ErrorMessage = "Telefoonnummer is verplicht.")]
    [RegularExpression(@"^(\+31|0)[1-9]\d{8}$", ErrorMessage = "Telefoonnummer moet een geldig Nederlands telefoonnummer zijn.")]
    public string telNummer { get; set; }

    [Required(ErrorMessage = "Bedrijfsnaam is verplicht.")]
    [StringLength(100, ErrorMessage = "Bedrijfsnaam mag niet langer zijn dan 100 tekens.")]
    public string bedrijfsNaam { get; set; }

    [Required(ErrorMessage = "wachtwoord is verplicht.")]
    [MinLength(8, ErrorMessage = "wachtwoord moet minimaal 8 tekens bevatten.")]
    public string wachtwoord { get; set; }

    [Required]
    public string AspNetUserId { get; set; }

    public bool IsActive { get; set; } = true;
}

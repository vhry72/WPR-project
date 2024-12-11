using System.ComponentModel.DataAnnotations;

namespace WPR_project.Models
{
    public class BedrijfsMedewerkers
    {
        [Key]
        // de bedrijfsmedewerkerId is hetzelfde als de zakelijkeHuurderId
        public Guid BedrijfsMedewerkId { get; set; }

        [Required(ErrorMessage = "Naam van medewerker is verplicht.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Medewerkernaam moet tussen 2 en 50 tekens zijn.")]
        public string medewerkerNaam { get; set; }

        [Required(ErrorMessage = "E-mailadres van medewerker is verplicht.")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string medewerkerEmail { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minimaal 8 tekens bevatten.")]
        [RegularExpression(@"^(?=.[A-Z])(?=.[!@#$&*]).+$", ErrorMessage = "Wachtwoord moet minstens één hoofdletter en één uniek teken bevatten.")]
        public string Wachtwoord { get; set; }

        public Guid ZakelijkeHuurderId { get; set; }
        public ZakelijkHuurder ZakelijkeHuurder { get; set; } // Relatie met zakelijke huurders
    }
}
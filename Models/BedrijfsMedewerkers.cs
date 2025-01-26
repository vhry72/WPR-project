using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPR_project.Models
{
    public class BedrijfsMedewerkers
    {
        [Key]
        public Guid bedrijfsMedewerkerId { get; set; } = Guid.NewGuid(); // Automatisch gegenereerde unieke ID

        [Required(ErrorMessage = "Naam van medewerker is verplicht.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Medewerkernaam moet tussen 2 en 50 tekens zijn.")]
        public string medewerkerNaam { get; set; } // Correcte PascalCase voor consistentie

        [Required(ErrorMessage = "E-mailadres van medewerker is verplicht.")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string medewerkerEmail { get; set; }

        public Guid EmailBevestigingToken { get; set; }

        public bool IsEmailBevestigd { get; set; } = false;

        [Required(ErrorMessage = "wachtwoord is verplicht.")]
        [MinLength(8, ErrorMessage = "wachtwoord moet minimaal 8 tekens bevatten.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$&*]).+$",
            ErrorMessage = "wachtwoord moet minstens één hoofdletter en één uniek teken bevatten.")]
        public string wachtwoord { get; set; }

        public Guid? AbonnementId { get; set; }


        public Guid zakelijkeId { get; set; } 

        public Guid beheerderId { get; set; }

        [Required]
        public string AspNetUserId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}


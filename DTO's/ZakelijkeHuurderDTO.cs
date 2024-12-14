using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace WPR_project.DTO_s
{
    public class ZakelijkeHuurderDTO
    {
        [Required(ErrorMessage = "Adres is verplicht.")]
        [StringLength(200, ErrorMessage = "Adres mag niet langer zijn dan 200 tekens.")]
        public string adres { get; set; }

        [Required(ErrorMessage = "KVK-nummer is verplicht.")]
        [Range(10000000, 99999999, ErrorMessage = "KVK-nummer moet een 8-cijferig getal zijn.")]
        public int KVKNummer { get; set; }

        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string bedrijfsEmail { get; set; }

        [Required(ErrorMessage = "Telefoonnummer is verplicht.")]
        [RegularExpression(@"^(\+31|0)[1-9]\d{8}$", ErrorMessage = "Telefoonnummer moet een geldig Nederlands telefoonnummer zijn.")]
        public string telNummer { get; set; }

        [Required(ErrorMessage = "Bedrijfsnaam is verplicht.")]
        [StringLength(100, ErrorMessage = "Bedrijfsnaam mag niet langer zijn dan 100 tekens.")]
        public string bedrijfsNaam { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minimaal 8 tekens bevatten.")]
        public string wachtwoord { get; set; }
    }

}

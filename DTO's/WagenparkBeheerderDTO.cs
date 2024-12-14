using System.ComponentModel.DataAnnotations;
using WPR_project.Models;

namespace WPR_project.DTO_s
{
    public class WagenparkBeheerderDTO
    {
        [Key]

        public int beheerderId { get; set; }

        [Required(ErrorMessage = "Beheerdersnaam is verplicht.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Beheerdersnaam mag niet langer zijn dan 100 tekens.")]
        public string beheerderNaam { get; set; }

        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string beheerderEmail { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minimaal 8 tekens bevatten.")]
        public string wachtwoord { get; set; }

    }
}

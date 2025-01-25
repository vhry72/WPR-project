using System.ComponentModel.DataAnnotations;

namespace WPR_project.DTO_s
{
    public class WagenparkBeheerderGetGegevensDTO
    {

        public Guid beheerderId { get; set; }

        [Required(ErrorMessage = "Beheerdersnaam is verplicht.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Beheerdersnaam mag niet langer zijn dan 100 tekens.")]
        public string beheerderNaam { get; set; }

        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string bedrijfsEmail { get; set; }

        public bool IsActive { get; set; }

    }
}

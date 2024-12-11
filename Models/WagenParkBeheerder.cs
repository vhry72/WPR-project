using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WPR_project.Models
{
    public class WagenparkBeheerder
    {
        [Key]
        public Guid beheerderId { get; set; }

        [Required(ErrorMessage = "Beheerdersnaam is verplicht.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Beheerdersnaam mag niet langer zijn dan 100 tekens.")]
        public string beheerderNaam { get; set; }

        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string bedrijfsEmail { get; set; }

        [Required(ErrorMessage = "Telefoonnummer is verplicht.")]
        [RegularExpression(@"^(+31|0)[1-9]\d{8}$", ErrorMessage = "Telefoonnummer moet een geldig Nederlands telefoonnummer zijn.")]
        public string telefoonNummer { get; set; }

        [Required(ErrorMessage = "De lijst van medewerkers mag niet leeg zijn.")]
        public List<BedrijfsMedewerkers> MedewerkerLijst { get; set; } = new List<BedrijfsMedewerkers>();
    }
}
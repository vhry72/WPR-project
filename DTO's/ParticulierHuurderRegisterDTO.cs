﻿using System.ComponentModel.DataAnnotations;

namespace WPR_project.DTO_s
{
    public class ParticulierHuurderRegisterDTO
    {

        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string particulierEmail { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Naam mag niet langer zijn dan 100 tekens.")]
        public string particulierNaam { get; set; }
        [Required(ErrorMessage = "wachtwoord is verplicht.")]
        [MinLength(8, ErrorMessage = "wachtwoord moet minimaal 8 tekens bevatten.")]
        public string wachtwoord { get; set; }

        [Required(ErrorMessage = "Adres is verplicht.")]
        [StringLength(200, ErrorMessage = "Adres mag niet langer zijn dan 200 tekens.")]
        public string adress { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht.")]
        [RegularExpression(@"^\d{4}[A-Z]{2}$", ErrorMessage = "Postcode moet het formaat 1234AB hebben.")]
        public string postcode { get; set; }

        [Required(ErrorMessage = "Woonplaats is verplicht.")]
        [StringLength(100, ErrorMessage = "Woonplaats mag niet langer zijn dan 100 tekens.")]
        public string woonplaats { get; set; }

        [Required(ErrorMessage = "Telefoonnummer is verplicht.")]
        [RegularExpression(@"^(\+31|0)[1-9]\d{8}$", ErrorMessage = "Telefoonnummer moet een geldig Nederlands telefoonnummer zijn.")]
        public string telefoonnummer { get; set; }
    }
}

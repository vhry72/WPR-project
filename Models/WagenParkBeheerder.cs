using Microsoft.EntityFrameworkCore;
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

        [Required(ErrorMessage = "Adres is verplicht.")]
        [StringLength(200, ErrorMessage = "Adres mag niet langer zijn dan 200 tekens.")]
        public string Adres { get; set; }

        [Required(ErrorMessage = "KVK-nummer is verplicht.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "KVK-nummer moet een 8-cijferig getal zijn.")]
        public int KVKNummer { get; set; }

        public string telefoonNummer { get; set; }

        [Required(ErrorMessage = "wachtwoord is verplicht.")]
        [MinLength(8, ErrorMessage = "wachtwoord moet minimaal 8 tekens bevatten.")]
        public string wachtwoord { get; set; }

        public Guid? AbonnementId { get; set; }
        public Abonnement? HuidigAbonnement { get; set; }

        public DateTime? updateDatumAbonnement { get; set; }

        public AbonnementType? AbonnementType { get; set; }

        public Guid EmailBevestigingToken { get; set; }

        public bool IsEmailBevestigd { get; set; } = false;

        [Precision(18, 2)]
        public decimal? PrepaidSaldo { get; set; } = 0;

        [Required]
        public string AspNetUserId { get; set; }

        public Guid zakelijkeId { get; set; }

        public int? voertuiglimiet { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
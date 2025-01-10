using System.ComponentModel.DataAnnotations;
using WPR_project.Models;

namespace WPR_project.DTO_s
{
    public class AbonnementDTO
    {
        [Required(ErrorMessage = "AbonnementId is verplicht.")]
        public Guid AbonnementId { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "De naam mag maximaal 100 tekens bevatten.")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "De begindatum is verplicht.")]
        public DateTime beginDatum { get; set; }

        [Required(ErrorMessage = "De vervaldatum is verplicht.")]
        [DataType(DataType.Date)]
        public DateTime vervaldatum { get; set; }

        [Required(ErrorMessage = "De kosten zijn verplicht.")]
        [Range(0, double.MaxValue, ErrorMessage = "Kosten moeten positief zijn.")]
        public decimal Kosten { get; set; }

        [Required(ErrorMessage = "BeheerderId is verplicht.")]
        public Guid beheerderId { get; set; }

        [Required(ErrorMessage = "Het type abonnement is verplicht.")]
        public AbonnementType Type { get; set; }

        [Required(ErrorMessage = "Het abonnementstermijn is verplicht.")]
        public AbonnementTermijnen AbonnementTermijnen { get; set; }

        [Required(ErrorMessage = "DirectZichtbaar moet worden gespecificeerd.")]
        public bool directZichtbaar { get; set; }

        [Range(0, 25, ErrorMessage = "De korting moet tussen 0 en 25% liggen.")]
        public decimal? korting { get; set; }
    }
}

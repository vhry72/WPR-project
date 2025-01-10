using System.ComponentModel.DataAnnotations;
using WPR_project.Models;

namespace WPR_project.DTO_s
{
    public class AbonnementDTO
    {
        
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "De naam mag maximaal 100 tekens bevatten.")]
        public string Naam { get; set; }


        [Required(ErrorMessage = "De kosten zijn verplicht.")]
        [Range(0, double.MaxValue, ErrorMessage = "Kosten moeten positief zijn.")]
        public decimal Kosten { get; set; }

        [Required(ErrorMessage = "BeheerderId is verplicht.")]
        public Guid zakelijkeId { get; set; }

        [Required(ErrorMessage = "Het type abonnement is verplicht.")]
        public AbonnementType Type { get; set; }

        [Required(ErrorMessage = "Het abonnementstermijn is verplicht.")]
        public AbonnementTermijnen AbonnementTermijnen { get; set; }

        public decimal? korting { get; set; }

    }
}

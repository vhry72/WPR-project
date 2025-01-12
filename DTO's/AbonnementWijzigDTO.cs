using System.ComponentModel.DataAnnotations;
using WPR_project.Models;
using Xunit.Sdk;

namespace WPR_project.DTO_s
{
    public class AbonnementWijzigDTO
    {

        public Guid AbonnementId { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "De naam mag maximaal 100 tekens bevatten.")]
        public string Naam { get; set; }


        [Required(ErrorMessage = "De kosten zijn verplicht.")]
        [Range(0, double.MaxValue, ErrorMessage = "Kosten moeten positief zijn.")]
        public decimal Kosten { get; set; }

        [Required(ErrorMessage = "BeheerderId is verplicht.")]
        public Guid zakelijkeId { get; set; }

        [Required(ErrorMessage = "Het type abonnement is verplicht.")]
        public AbonnementType AbonnementType { get; set; }

        [Required(ErrorMessage = "Het abonnementstermijn is verplicht.")]
        public AbonnementTermijnen AbonnementTermijnen { get; set; }

        public decimal? korting { get; set; }

        public int? AantalDagen { get; set; }

        public bool? directZichtbaar { get; set; }

        public DateTime? updateDatum { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace WPR_project.Models
{
    public class Huurverzoek
    {
        public Guid HuurderID { get; set; }
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }
        public bool approved { get; set; }
        public bool isBevestigd { get; set; }
        public string? Reden { get; set; }

        [ForeignKey("Voertuig")]
        public Guid VoertuigId { get; set; } // Foreign key voor het voertuig

        public Voertuig Voertuig { get; set; } // Navigatieproperty
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace WPR_project.Models
{
    public class Huurverzoek
    {
        public Guid HuurVerzoekId { get; set; }
        public Guid HuurderID { get; set; }
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }
        public bool approved { get; set; }
        public bool isBevestigd { get; set; }
        public string? Reden { get; set; }

        [ForeignKey("Voertuig")]
        public Guid VoertuigId { get; set; } 

        public Voertuig Voertuig { get; set; }
    }
}

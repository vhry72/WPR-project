using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPR_project.Models
{
    public class Schademelding
    {
        [Key]
        public Guid SchademeldingId { get; set; } 

        [Required]
        public string Beschrijving { get; set; } 

        [Required]
        public DateTime Datum { get; set; } 

        [Required]
        public string Status { get; set; } 

        public string Opmerkingen { get; set; } 

        public SoortOnderhoud SoortOnderhoud { get; set; }
        
        [Required]
        public Guid VoertuigId { get; set; }

        [ForeignKey("VoertuigId")]
        public Voertuig Voertuig { get; set; }
    }
}


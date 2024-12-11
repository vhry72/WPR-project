using System.ComponentModel.DataAnnotations;
namespace WPR_project.Models
{
    public class VoertuigStatus
    {
        [Key]
        public Guid VoertuigStatusId { get; set; }
        public bool verhuurd { get; set; }
        public bool schade { get; set; }
        public bool onderhoud { get; set; }

        public Guid voertuigId { get; set; } //relatie met Voertuig klasse
    }
}

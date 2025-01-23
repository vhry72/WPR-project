using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace WPR_project.Models
{
    public class FrontofficeMedewerker 
    {
        public Guid FrontofficeMedewerkerId { get; set; }
        public string medewerkerNaam { get; set; }
        public string medewerkerEmail { get; set; }
        public string wachtwoord { get; set; }

        public Guid EmailBevestigingToken { get; set; }

        public bool IsEmailBevestigd { get; set; } = false;

        [Required]
        public string AspNetUserId { get; set; }

        public bool IsActive { get; set; } = true;

        [JsonIgnore] // Zorg ervoor dat Medewerkers niet wordt opgenomen in de JSON
        public List<FrontofficeMedewerker>? FrontofficeMedewerkers { get; set; } = new List<FrontofficeMedewerker>();
    }
}
 
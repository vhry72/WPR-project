using System.ComponentModel.DataAnnotations;
namespace WPR_project.Models
{
    public class FrontofficeMedewerker 
    {
        public Guid FrontofficeMedewerkerId { get; set; }
        public string medewerkerNaam { get; set; }
        public string medewerkerEmail { get; set; }
        public string wachtwoord { get; set; }

        [Required]
        public string AspNetUserId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WPR_project.Models
{
    public class BackofficeMedewerker
    {
        public Guid BackofficeMedewerkerId { get; set; }
        public string medewerkerNaam { get; set; }
        public string medewerkerEmail { get; set; }

        public string wachtwoord { get; set; }

        [Required]
        public string AspNetUserId { get; set; }
    }
}

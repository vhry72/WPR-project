using System.ComponentModel.DataAnnotations;
namespace WPR_project.Models
{
    public class FrontofficeMedewerker : Medewerker
    {
        [Required]
        public string AspNetUserId { get; set; }
    }
}

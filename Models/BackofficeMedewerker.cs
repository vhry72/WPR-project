using System.ComponentModel.DataAnnotations;
namespace WPR_project.Models
{
    public class BackofficeMedewerker : Medewerker
    {
        [Required]
        public string AspNetUserId { get; set; }
    }

}

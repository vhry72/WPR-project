using System.ComponentModel.DataAnnotations;
using WPR_project.Models;
namespace WPR_project.DTO_s;

public class VoertuigStatusDTO
{
    [Required]
    public Guid VoertuigStatusId { get; set; }
    public bool verhuurd { get; set; }
    public bool schade { get; set; }
    public bool onderhoud { get; set; }
    public Guid voertuigId { get; set; }
    public Voertuig voertuig { get; set; }
}
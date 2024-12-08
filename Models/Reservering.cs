using WPR_project.Models;

public class Reservering
{
    public Guid ReserveringId { get; set; }
    public int VoertuigId { get; set; }
    public Voertuig Voertuig { get; set; }
    public DateTime StartDatum { get; set; }
    public DateTime EindDatum { get; set; }
}
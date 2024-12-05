namespace WPR_project.Models
{
    public class VoertuigStatus
    {
        public int VoertuigStatusId { get; set; }
        public bool verhuurd { get; set; }
        public bool schade { get; set; }
        public bool onderhoud { get; set; }
    }
}

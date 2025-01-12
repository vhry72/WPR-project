namespace WPR_project.Models
{
    public class VoertuigNotities
    {
        public Guid NotitieId { get; set; }
        public Guid voertuigId { get; set; }

        public string notitie { get; set; }

        public DateTime NotitieDatum { get; set; }
    }
}

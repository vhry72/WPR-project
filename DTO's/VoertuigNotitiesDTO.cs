namespace WPR_project.DTO_s
{
    public class VoertuigNotitiesDTO
    {
        public Guid NotitieId { get; set; }
        public Guid voertuigId { get; set; }

        public string notitie { get; set; }

        public DateTime NotitieDatum { get; set; }
    }
}

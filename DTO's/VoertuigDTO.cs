namespace WPR_project.DTO_s
{
    public class VoertuigDTO
    {
        public Guid voertuigId { get; set; }
        public DateTime? StartDatum { get; set; }
        public DateTime? EindDatum { get; set; }
        public bool voertuigBeschikbaar { get; set; }

        public string merk { get; set; }
        public string model { get; set; }
        public string kenteken { get; set; }
        public string voertuigType { get; set; }
        public VoertuigStatusDTO voertuigStatus { get; set; }

    }
}


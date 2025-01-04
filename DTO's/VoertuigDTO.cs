namespace WPR_project.DTO_s
{
    public class VoertuigDTO
    {
        public DateTime? StartDatum { get; set; }
        public DateTime? EindDatum { get; set; }
        public bool VoertuigBeschikbaar { get; set; }

        public string merk { get; set; }
        public string model { get; set; }


    }
}


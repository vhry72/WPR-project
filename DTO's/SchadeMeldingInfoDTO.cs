namespace WPR_project.DTO_s
{
    public class SchadeMeldingInfoDTO
    {
        public Guid SchademeldingId { get; set; }
        public string Beschrijving { get; set; }
        public DateTime Datum { get; set; }
        public string Status { get; set; }
        public string Opmerkingen { get; set; }

        public Guid VoertuigId { get; set; }

        public bool? IsAfgehandeld { get; set; }

    }
}

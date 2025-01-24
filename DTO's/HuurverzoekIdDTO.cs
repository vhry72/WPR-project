namespace WPR_project.DTO_s
{
    public class HuurverzoekIdDTO
    {


        public Guid HuurVerzoekId { get; set; }

        public Guid HuurderID { get; set; } // Verwijzing naar de huurder
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }

        public bool isBevestigd { get; set; }


        public bool approved { get; set; }

    }
}

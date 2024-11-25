namespace WPR_project.Models
{
    public class Huurverzoek
    {
        public int Id { get; set; }
        public string HuurderID { get; set; }
        public string VoertuigId { get; set; }
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }

        public bool approved { get; set; }
    }
}

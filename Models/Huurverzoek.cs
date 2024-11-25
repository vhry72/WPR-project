namespace WPR_project.Models
{
    public class Huurverzoek
    {
        public string HuurderID { get; set; }
        public string voertuigId { get; set; }
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }

        public bool approved { get; set; }
    }
}

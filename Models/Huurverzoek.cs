namespace WPR_project.Models
{
    public class Huurverzoek
    {
        public Guid HuurderID { get; set; }
        public Guid voertuigId { get; set; }
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }

        public bool approved { get; set; }
    }
}

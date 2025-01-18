namespace WPR_project.Models
{
    public class PrivacyVerklaring
    {
        public Guid VerklaringId { get; set; }

        public Guid MedewerkerId { get; set; }

        public DateTime UpdateDatum { get; set; }

        public string Verklaring { get; set; }
    }
}

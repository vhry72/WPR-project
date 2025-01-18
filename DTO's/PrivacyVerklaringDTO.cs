namespace WPR_project.DTO_s
{
    public class PrivacyVerklaringDTO
    {
        public Guid VerklaringId { get; set; }

        public Guid MedewerkerId { get; set; }

        public DateTime UpdateDatum { get; set; }

        public string Verklaring { get; set; }
    }
}

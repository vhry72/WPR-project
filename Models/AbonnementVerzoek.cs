namespace WPR_project.Models
{
    public class AbonnementVerzoek
    {
        public Guid AbonnementId { get; set; }
        public AbonnementType AbonnementType { get; set; }
        public bool directZichtbaar { get; set; }
        public bool volgendePeriode { get; set; }
    }
}
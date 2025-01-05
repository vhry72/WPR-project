namespace WPR_project.Models
{
    public class AbonnementVerzoek
    {
        public Guid AbonnementId { get; set; }
        public AbonnementType AbonnementType { get; set; }
        public bool directZichtbaar { get; set; }
        public int aantalDagen { get; set; }
        public decimal korting { get; set; }
        public string details { get; set; }
        public bool volgendePeriode { get; set; }
    }
}
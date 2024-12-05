namespace WPR_project.Models
{
    public class Abonnement
    {
        public int AbonnementId { get; set; }
        public string abonnementNaam { get; set; }
        public string price { get; set; }

        public string term { get; set; }
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }

    }
}

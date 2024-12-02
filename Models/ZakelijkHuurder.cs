namespace WPR_project.Models
{
    public class ZakelijkHuurder
    {
        public int zakelijkeId { get; set; }  
        public string adres { get; set; }
        public int KVKNummer { get; set; }
        public string email { get; set; }
        public int telNummer { get; set; }
        public string bedrijfsNaam { get; set;}

        public List<string> MedewerkersEmails { get; set; } = new List<string>();

    }
}

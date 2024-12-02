namespace WPR_project.Models
{
    public class ZakelijkHuurder : Huurder
    {

        public string bedrijfsNaam { get; set;}

        public List<string> MedewerkersEmails { get; set; } = new List<string>();

    }
}

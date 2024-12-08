namespace WPR_project.Models
{
    public class BedrijfsMedewerkers : ZakelijkHuurder
    {

        public int BedrijfsMedewerkId { get; set; }
        public string medewerkerNaam {  get; set; }
        public string medewerkerEmail { get; set; }
        public string Wachtwoord { get; set; }
    }
}

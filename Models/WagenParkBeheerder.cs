namespace WPR_project.Models
{
    public class WagenparkBeheerder
    {
        public int beheerderId { get; set; }
        public string beheerderNaam { get; set; }
        public string email { get; set; }
        public int telNummer { get; set; }

        public List<BedrijfsMedewerkers>MedewerkerLijst = new List<BedrijfsMedewerkers>   {  };


    }
}

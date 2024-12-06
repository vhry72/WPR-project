namespace WPR_project.Models
{
    public abstract class Huurder
    {
        public int HuurderId { get; set; }
        public string adress { get; set; }

        public string naam { get; set; }

        public string email { get; set; }
        public int telefoonNr { get; set; }

        public string wachtwoord { get; set; }
    }
}

using System.Security.Cryptography.X509Certificates;

namespace WPR_project.Models
{
    public class ParticulierHuurder
    {
        public int gebruikerId { get; set; }
        public string gebruikerAdres { get; set; }
        public string gebruikerMail { get; set; }
        public int telNummer { get; set; }

        //public string BevestigingsMail()
        //{
        //    if (gebruikerMail != null)
        //    {
        //        return gebruikerMail;
        //    }
        //    else
        //    {
        //        return "Geen E-mail ingevoerd!";
        //    }
        //}
    }
}

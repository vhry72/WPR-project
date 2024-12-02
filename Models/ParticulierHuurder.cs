using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace WPR_project.Models { 
    public class ParticulierHuurder
    { 
        public int ParticulierId { get; set; }
        public string particulierNaam { get; set; }
        public string particulierEmail { get; set; }
        public int telefoonNummer { get; set; }


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

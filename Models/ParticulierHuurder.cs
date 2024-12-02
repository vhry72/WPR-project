using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace WPR_project.Models { 
    public class ParticulierHuurder //: Huurder
    {
        public int particulierId { get; set; }
        public string particulierEmail { get; set; }
        public string particulierNaam { get; set; }



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

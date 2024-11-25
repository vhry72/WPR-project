using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace WPR_project.Models { 
    public class ParticulierHuurder
    { 
        public int gebruikerId { get; set; }
        public string naam { get; set; }
        public string email { get; set; }
        public int phoneNumber { get; set; }


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

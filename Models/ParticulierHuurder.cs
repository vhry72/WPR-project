﻿using System.Security.Cryptography.X509Certificates;
using System.Transactions;

namespace WPR_project.Models
{
    public class ParticulierHuurder
    {
        public int particulierId { get; set; }
        public string particulierEmail { get; set; }
        public string particulierNaam { get; set; }
        public string EmailBevestigingToken { get; set; }
        public bool IsEmailBevestigd { get; set; } = false;

        public string wachtwoord { get; set; }

        public string adress { get; set; }
        public string postcode { get; set; }
        public string woonplaats { get; set; }

        public string telefoonnummer { get; set; }
    }
}

﻿namespace WPR_project.Models
{
    public abstract class Medewerker
    {
        public int medewerkerId { get; set; }
        public string medewerkerNaam { get; set; }
        public string medewerkerEmail { get; set; }
        public string medewerkerRol { get; set; }
    }
}

﻿using Microsoft.EntityFrameworkCore;

namespace WPR_project.Models
{
    public class Voertuig
    {
        public int voertuigId { get; set; }
        public string merk { get; set; }
        public string model { get; set; }

        [Precision(18, 2)]
        public decimal PrijsPerDag { get; set; }

        public string voertuigType { get; set; }

    }
}

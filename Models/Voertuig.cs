using Microsoft.EntityFrameworkCore;

namespace WPR_project.Models
{
    public class Voertuig
    {
        public int voertuigId { get; set; }
        public string merk { get; set; }
        public string model { get; set; }

        public string kenteken { get; set; }

        public string kleur { get; set; }

        public int bouwjaar { get; set; }

        [Precision(18, 2)]
        public decimal prijsPerDag { get; set; }
        public string voertuigType { get; set; }

    }
}

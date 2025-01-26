using Microsoft.EntityFrameworkCore;

namespace WPR_project.DTO_s
{
    public class VoertuigWijzigingDTO
    {
        public string merk { get; set; }


        public string model { get; set; }


        public string kleur { get; set; }


        [Precision(18, 2)]
        public decimal prijsPerDag { get; set; }

        public int bouwjaar { get; set; }

        public string kenteken { get; set; }

        public int? AantalDeuren { get; set; }

        public int? AantalSlaapplekken { get; set; }

        public byte[]? Afbeelding { get; set; }
    }
}

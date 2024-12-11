using Microsoft.EntityFrameworkCore;

namespace WPR_project.Models
{
    public class Voertuig
    {
        public Guid voertuigId { get; set; }
        public string merk { get; set; }
        public string model { get; set; }
        public string kleur { get; set; }

        [Precision(18, 2)]
        public decimal prijsPerDag { get; set; }
        public string voertuigType { get; set; }
        public int bouwjaar { get; set; }
        public string kenteken { get; set; }
        public DateTime startDatum { get; set; }
        public DateTime eindDatum { get; set; }
        public bool voertuigBeschikbaar { get; set; }

        public VoertuigStatus voertuigstatus { get; set; }
    }
}

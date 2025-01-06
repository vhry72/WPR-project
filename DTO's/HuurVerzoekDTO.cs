using System;

namespace WPR_project.DTO_s
{
    public class HuurVerzoekDTO
    {
        public Guid HuurderID { get; set; } // Verwijzing naar de huurder

        public Guid VoertuigId { get; set; } // Verwijzing naar het voertuig
        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }

        public bool approved { get; set; }

        public bool isBevestigd { get; set; }

        public string? reden { get; set; }
    }
}

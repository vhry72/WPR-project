using Microsoft.Identity.Client;

namespace WPR_project.DTO_s
{
    public class ZakelijkeHuurderDTO
    {
        public int zakelijkeId { get; set; }
        public string zakelijkeEmail { get; set; }
        public string zakelijkAdres { get; set; }
        public string bedrijfsnaam { get; set; }
    }
}

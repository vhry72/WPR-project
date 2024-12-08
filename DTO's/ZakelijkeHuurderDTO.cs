using Microsoft.Identity.Client;

namespace WPR_project.DTO_s
{
    public class ZakelijkeHuurderDTO
    {
        public Guid zakelijkeId { get; set; }
        public string adres { get; set; }
        public int KVKNummer { get; set; }
        public string email { get; set; }
        public int telNummer { get; set; }
        public string bedrijfsnaam { get; set; }
    }
}

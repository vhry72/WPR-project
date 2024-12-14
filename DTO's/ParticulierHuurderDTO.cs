namespace WPR_project.DTO_s
{
    public class ParticulierHuurderDTO 
    {
       public Guid particulierId { get; set; }
        public string particulierEmail { get; set; }
        public string particulierNaam { get; set; }
        public string wachtwoord { get; set; }
        public bool IsEmailBevestigd { get; internal set; }
    }
}

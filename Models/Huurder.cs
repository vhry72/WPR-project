namespace WPR_project.Models
{
    public abstract class Huurder
    {
        public int Id { get; set; }
        public string adress { get; set; }

        public string naam { get; set; }

        public string email { get; set; }
        public int telefoonNr { get; set; }
    }
}

using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IPrivacyVerklaringRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        public PrivacyVerklaring GetLatestPrivacyVerklaring();

        public void Add(PrivacyVerklaring privacyVerklaring);

        IQueryable<PrivacyVerklaring> GetAllPrivacyVerklaringen();

    }
}

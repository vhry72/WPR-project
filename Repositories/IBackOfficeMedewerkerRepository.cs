using WPR_project.Models;

namespace WPR_project.Repositories
{
    // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

    public interface IBackOfficeMedewerkerRepository
    {
        public BackofficeMedewerker GetBackofficemedewerkerById(Guid id);

        void AddBackOfficeMedewerker(BackofficeMedewerker backOfficeMedewerker);

        void UpdateBackOfficeMedewerker(BackofficeMedewerker backOfficeMedewerker, Guid id);

        void DeleteBackOfficeMedewerker(Guid id);

        void Save();
    }
}

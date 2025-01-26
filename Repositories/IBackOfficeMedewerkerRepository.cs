using WPR_project.Models;

namespace WPR_project.Repositories
{
    // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

    public interface IBackOfficeMedewerkerRepository
    {
        public BackofficeMedewerker GetBackofficemedewerkerById(Guid id);

        void UpdateBackOfficeMedewerker(BackofficeMedewerker backOfficeMedewerker);

        void DeleteBackOfficeMedewerker(Guid id);

        void DeactivateBackOfficeMedewerker(Guid id);

        void AbonnementKeuring(Guid abonnementId, bool keuring);

        void Save();
    }
}

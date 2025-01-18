using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IBackOfficeMedewerkerRepository
    {
        public BackofficeMedewerker GetBackofficemedewerkerById(Guid id);

        void AddBackOfficeMedewerker(BackofficeMedewerker backOfficeMedewerker);

        void UpdateBackOfficeMedewerker(BackofficeMedewerker backOfficeMedewerker, Guid id);

        void DeleteBackOfficeMedewerker(Guid id);

        void Save();
    }
}

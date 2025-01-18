using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IBackOfficeMedewerkerRepository
    {
        public BackofficeMedewerker GetBackofficemedewerkerById(Guid id);
    }
}

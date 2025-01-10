using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IFrontOfficeMedewerkerRepository
    {
        FrontofficeMedewerker GetFrontOfficeMedewerkerById(Guid id);
        void Update(FrontofficeMedewerker frontofficeMedewerker);
        void Save();
    }
}

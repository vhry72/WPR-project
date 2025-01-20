using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IFrontOfficeMedewerkerRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        FrontofficeMedewerker GetFrontOfficeMedewerkerById(Guid id);
        void Update(FrontofficeMedewerker frontofficeMedewerker);
        void Save();
    }
}

using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IVoertuigNotitiesRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        IEnumerable<VoertuigNotities> GetVoertuigNotitiesByVoertuigId(Guid voertuigId);

        void AddVoertuigNotitie(VoertuigNotities voertuigNotities);

        void Save();
        
    }
}

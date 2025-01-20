using WPR_project.Models;
using WPR_project.DTO_s;
namespace WPR_project.Repositories
{
    public interface IVoertuigStatusRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        IQueryable<VoertuigStatus> GetAllVoertuigenStatussen();
        public VoertuigStatus GetByID(Guid Id);
        void Update(VoertuigStatus voertuigStatus);
        void Save();
    }
}

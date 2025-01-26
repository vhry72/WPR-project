using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IVoertuigRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB
        IEnumerable<Voertuig> GetFilteredVoertuigen(string voertuigType, DateTime? startDatum, DateTime? eindDatum, string sorteerOptie);

        IEnumerable<Voertuig> GetVoertuigTypeVoertuigen(string voertuigType);

        Voertuig GetVoertuigById(Guid id);

        public Voertuig GetByID(Guid Id);
        void updateVoertuig(Voertuig voertuig);
        void Delete(Guid id);
        void Add(Voertuig voertuig);
        void Save();
        
    }
}

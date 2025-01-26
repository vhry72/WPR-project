using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IHuurderRegistratieRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        ParticulierHuurder GetById(Guid id);
        void Update(ParticulierHuurder particulierHuurder);
        void Delete(Guid id);
        void DectivateParticulier(Guid id);
        void Save();
    }
}
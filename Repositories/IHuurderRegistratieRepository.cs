using WPR_project.Data;
using WPR_project.Models;
using WPR_project.DTO_s;

namespace WPR_project.Repositories
{
    public interface IHuurderRegistratieRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        IEnumerable<ParticulierHuurder> GetAll();
        ParticulierHuurder GetById(Guid id);
        ParticulierHuurder GetByEmailAndPassword(string email, string password);

        ParticulierHuurder GetByToken(Guid token);
        void Add(ParticulierHuurder particulierHuurder);
        void Update(ParticulierHuurder particulierHuurder);
        void Delete(Guid id);
        void Save();
    }
}
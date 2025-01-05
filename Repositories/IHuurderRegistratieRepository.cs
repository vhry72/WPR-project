using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IHuurderRegistratieRepository
    {
        IEnumerable<ParticulierHuurder> GetAll();
        ParticulierHuurder GetById(Guid id);
        ParticulierHuurder GetByEmailAndPassword(string email, string password);
        ParticulierHuurder GetByToken(Guid token);
        void Add(ParticulierHuurder particulierHuurder);
        void Update(ParticulierHuurder particulierHuurder);
        void Delete(Guid id);
        void Save();
        void Delete(ParticulierHuurder huurder);
    }
}
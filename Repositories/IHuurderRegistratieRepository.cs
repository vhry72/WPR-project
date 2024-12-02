using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IHuurderRegistratieRepository
    {
        IEnumerable<ParticulierHuurder> GetAll();
        ParticulierHuurder GetById(int id);
        void Add(ParticulierHuurder particulierHuurder);
        void Update(ParticulierHuurder particulierHuurder);
        void Delete(int id);
        void Save();
    }
}
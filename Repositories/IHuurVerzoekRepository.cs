using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IHuurVerzoekRepository
    {
        IEnumerable<HuurVerzoek> GetAllHuurVerzoeken();
        public HuurVerzoek GetByID(Guid Id);
        void Update(HuurVerzoek huurVerzoek);
    }
}

using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IHuurVerzoekRepository
    {
        void Add(Huurverzoek huurverzoek);

        IQueryable<Huurverzoek> GetActiveHuurverzoekenByHuurderId(Guid huurderId);

        IQueryable<Huurverzoek> GetHuurverzoekenForReminder(DateTime reminderTime);

        IQueryable<Huurverzoek> GetAllHuurVerzoeken();
        IQueryable<Huurverzoek> GetAllActiveHuurVerzoeken();
        IQueryable<Huurverzoek> GetAllBeantwoordenHuurVerzoeken();
        IQueryable<Huurverzoek> GetAllAfgekeurde();
        public Huurverzoek GetByID(Guid Id);
        void Update(Huurverzoek huurVerzoek);
        void Save();
    }
}

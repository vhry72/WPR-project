using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IHuurVerzoekRepository
    {
        void Add(Huurverzoek huurverzoek);

        IEnumerable<Huurverzoek> GetActiveHuurverzoekenByHuurderId(Guid huurderId);

        IEnumerable<Huurverzoek> GetHuurverzoekenForReminder(DateTime reminderTime);

        IEnumerable<Huurverzoek> GetAllHuurVerzoeken();
        IEnumerable<Huurverzoek> GetAllActiveHuurVerzoeken();
        public Huurverzoek GetByID(Guid Id);
        void Update(Huurverzoek huurVerzoek);
        void Save();
    }
}

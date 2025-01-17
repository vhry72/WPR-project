using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IHuurVerzoekRepository
    {
        void Add(Huurverzoek huurverzoek);

        IQueryable<Huurverzoek> GetActiveHuurverzoekenByHuurderId(Guid huurderId);
        IQueryable<Huurverzoek> GetBeantwoordeHuurverzoekenByHuurderId(Guid huurderId);

        IQueryable<Huurverzoek> GetHuurverzoekenForReminder(DateTime reminderTime);

        IQueryable<Voertuig> GetAvailableVehicles(DateTime startDatum, DateTime eindDatum);

        IQueryable<Huurverzoek> GetAllHuurVerzoeken();

        List<Huurverzoek> GetHuurverzoekenByHuurderID(Guid huurderId);
        IQueryable<Huurverzoek> GetHuurverzoekIdByHuurderID(Guid id);
        IQueryable<Huurverzoek> GetAllActiveHuurVerzoeken();
        IQueryable<Huurverzoek> GetAllBeantwoorde();
        IQueryable<Huurverzoek> GetAllAfgekeurde();
        IQueryable<Huurverzoek> GetAllGoedGekeurde();

        IQueryable<Huurverzoek> GetById(Guid id);
        public Huurverzoek GetByID(Guid Id);
        void Update(Huurverzoek huurVerzoek);
        void Save();
    }
}

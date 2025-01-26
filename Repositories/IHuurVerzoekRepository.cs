using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IHuurVerzoekRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        void Add(Huurverzoek huurverzoek);

        IQueryable<Voertuig> GetAvailableVehicles(DateTime startDatum, DateTime eindDatum);

        IQueryable<Huurverzoek> GetAllHuurVerzoeken();

        List<Huurverzoek> GetHuurverzoekenByHuurderID(Guid huurderId);
        IQueryable<Huurverzoek> GetAllActiveHuurVerzoeken();
        IQueryable<Huurverzoek> GetAllBeantwoorde();
        IQueryable<Huurverzoek> GetAllAfgekeurde();
        IQueryable<Huurverzoek> GetAllGoedGekeurde();
        public Huurverzoek GetByID(Guid Id);
        void Update(Huurverzoek huurVerzoek);
        void Save();
    }
}

using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IWagenparkBeheerderRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        IEnumerable<WagenparkBeheerder> GetWagenparkBeheerders();
        WagenparkBeheerder GetBeheerderById(Guid id); 
        void AddWagenparkBeheerder(WagenparkBeheerder wagenParkBeheerder);
        void UpdateWagenparkBeheerder(WagenparkBeheerder wagenParkBeheerder, Guid id);
        void DeleteWagenparkBeheerder(Guid id);
        void SetWagenparkBeheerderInactive(Guid id);

        Guid GetZakelijkeId(Guid id);

        Guid GetAbonnementId(Guid id);

        List<Guid> GetMedewerkersIdsByWagenparkbeheerder(Guid wagenparkbeheerderId);
        List<BedrijfsMedewerkers> GetMedewerkersByWagenparkbeheerder(Guid wagenparkbeheerderId);

        void updateWagenparkBeheerderGegevens(WagenparkBeheerder wagenparkbeheerder);


        //sla op 
        void Save();
        void UpdateWagenparkBeheerder(WagenparkBeheerder existingBeheerder);
    }
}

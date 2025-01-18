using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IWagenparkBeheerderRepository
    {
        IEnumerable<WagenparkBeheerder> GetWagenparkBeheerders();
        WagenparkBeheerder GetBeheerderById(Guid id); 
        void AddWagenparkBeheerder(WagenparkBeheerder wagenParkBeheerder);
        void UpdateWagenparkBeheerder(WagenparkBeheerder wagenParkBeheerder, Guid id);
        void DeleteWagenparkBeheerder(Guid id);

        Guid GetZakelijkeId(Guid id);

        Guid GetAbonnementId(Guid id);

        List<Guid> GetMedewerkersIdsByWagenparkbeheerder(Guid wagenparkbeheerderId);
        List<BedrijfsMedewerkers> GetMedewerkersByWagenparkbeheerder(Guid wagenparkbeheerderId);

        //sla op 
        void Save();
        void UpdateWagenparkBeheerder(WagenparkBeheerder existingBeheerder);
    }
}

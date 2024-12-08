using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IWagenparkBeheerderRepository
    {
        IEnumerable<WagenparkBeheerder> GetWagenparkBeheerders();
        WagenparkBeheerder getBeheerderById(Guid id); 
        void AddWagenparkBeheerder(WagenparkBeheerder wagenParkBeheerder);
        void UpdateWagenparkBeheerder(WagenparkBeheerder wagenParkBeheerder, Guid id);
        void DeleteWagenparkBeheerder(Guid id);

        //sla op 
        void Save();
        void UpdateWagenparkBeheerder(WagenparkBeheerder existingBeheerder);
    }
}

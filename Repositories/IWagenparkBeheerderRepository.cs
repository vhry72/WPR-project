using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IWagenparkBeheerderRepository
    {
        IEnumerable<WagenparkBeheerder> GetWagenparkBeheerders();
        WagenparkBeheerder getBeheerderById(int id); 
        void AddWagenparkBeheerder(WagenparkBeheerder wagenParkBeheerder);
        void UpdateWagenparkBeheerder(WagenparkBeheerder wagenParkBeheerder, int id);
        void DeleteWagenparkBeheerder(int id);

        //sla op 
        void Save();
        void UpdateWagenparkBeheerder(WagenparkBeheerder existingBeheerder);
    }
}

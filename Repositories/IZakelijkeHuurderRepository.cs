using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IZakelijkeHuurderRepository
    {
        // Functions for ZakelijkHuurder
        IEnumerable<ZakelijkHuurder> GetAllZakelijkHuurders();
        ZakelijkHuurder GetZakelijkHuurderById(int id);
        void AddZakelijkHuurder(ZakelijkHuurder zakelijkHuurder);
        void UpdateZakelijkHuurder(ZakelijkHuurder zakelijkHuurder);
        void DeleteZakelijkHuurder(int id);

        // Standard save
        void Save();
    }
}

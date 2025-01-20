using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IZakelijkeHuurderRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB
        IEnumerable<ZakelijkHuurder> GetAllZakelijkHuurders();
        ZakelijkHuurder GetZakelijkHuurderById(Guid id);
        ZakelijkHuurder GetZakelijkHuurderByToken(Guid token);

        Guid? GetAbonnementIdByZakelijkeHuurder(Guid id);

        void AddZakelijkHuurder(ZakelijkHuurder zakelijkHuurder);
        void UpdateZakelijkHuurder(ZakelijkHuurder zakelijkHuurder);
        void DeleteZakelijkHuurder(Guid id);

        // Standard save
        void Save();
    }
}

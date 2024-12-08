using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IBedrijfsMedewerkersRepository
    {
        void Add(BedrijfsMedewerkers medewerker);
        void Delete(int medewerkerId);
        BedrijfsMedewerkers GetMedewerkerById(int medewerkerId);
        IEnumerable<BedrijfsMedewerkers> GetAll();
        void Save();
    }
}

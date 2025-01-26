

namespace WPR_project.Repositories
{
    public interface IAbonnementRepository
    {
        IEnumerable<Abonnement> GetAllAbonnementen();
        Abonnement GetAbonnementById(Guid id);
        void AddAbonnement(Abonnement abonnement);
        void UpdateAbonnement(Abonnement abonnement);
        void Save();
    }
}
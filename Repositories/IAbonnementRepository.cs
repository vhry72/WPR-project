using System.Diagnostics.Contracts;

namespace WPR_project.Repositories
{
    public interface IAbonnementRepository
    {
        IEnumerable<Abonnement> GetAllAbonnementen();
        IEnumerable<Abonnement> GetBijnaVerlopenAbonnementen();
        Abonnement GetAbonnementById(Guid id);
        void AddAbonnement(Abonnement abonnement);
        void UpdateAbonnement(Abonnement abonnement);
        void DeleteAbonnement(Guid id);
        void Save();
    }
}
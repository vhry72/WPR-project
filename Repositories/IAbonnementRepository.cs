using WPR_project.DTO_s;

namespace WPR_project.Repositories
{
    public interface IAbonnementRepository
    {
        IEnumerable<Abonnement> GetAllAbonnementen();
        Abonnement GetAbonnementById(Guid id);
        void AddAbonnement(Abonnement abonnement);
        void UpdateAbonnement(Abonnement abonnement);

        public void UpdateAbonnementMetHangfire(AbonnementWijzigDTO dto);

        public void UpdateInToekomst(AbonnementWijzigDTO dto);

        void Save();
    }
}
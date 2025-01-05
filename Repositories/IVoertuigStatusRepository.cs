using WPR_project.Models;
using WPR_project.DTO_s;
namespace WPR_project.Repositories
{
    public interface IVoertuigStatusRepository
    {
        IQueryable<VoertuigStatus> GetAllVoertuigenStatussen();
        public VoertuigStatus GetByID(Guid Id);
        void Update(VoertuigStatus voertuigStatus);
        void Save();
    }
}

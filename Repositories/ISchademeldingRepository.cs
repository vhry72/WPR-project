using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface ISchademeldingRepository
    {
        IEnumerable<Schademelding> GetAllSchademeldingen();
        List<Schademelding> GetSchademeldingByVoertuigId(Guid id);
        Schademelding GetSchademeldingById(Guid id);
        void updateSchademelding(Schademelding schademelding);
        void Add(Schademelding schademelding);
        void Save();
    }
}

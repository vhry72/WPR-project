using WPR_project.DTO_s;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface ISchademeldingRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        IQueryable<Schademelding> GetAllSchademeldingen();
        List<Schademelding> GetSchademeldingByVoertuigId(Guid id);
        Schademelding GetSchademeldingById(Guid id);
        void updateSchademelding(Schademelding schademelding);
        void Add(Schademelding schademelding);
        void Update(Schademelding schademelding);
        void Save();
    }
}

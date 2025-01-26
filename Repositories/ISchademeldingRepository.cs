using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface ISchademeldingRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        IQueryable<Schademelding> GetAllSchademeldingen();
        Schademelding GetSchademeldingById(Guid id);
        void Add(Schademelding schademelding);
        void Update(Schademelding schademelding);
        void Save();
    }
}

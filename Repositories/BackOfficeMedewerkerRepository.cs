using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class BackOfficeMedewerkerRepository : IBackOfficeMedewerkerRepository
    {
        private readonly GegevensContext _context;

        public BackOfficeMedewerkerRepository(GegevensContext context)      
        {
            _context = context;
        }

        public BackofficeMedewerker GetBackofficemedewerkerById(Guid id)
        {
            return _context.BackofficeMedewerkers.Find(id);
        }
    }
}

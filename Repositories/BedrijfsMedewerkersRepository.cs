using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{

    public class BedrijfsMedewerkersRepository : IBedrijfsMedewerkersRepository
    {
        private readonly GegevensContext _context;

        public BedrijfsMedewerkersRepository(GegevensContext context)
        {
            _context = context;
        }

        public void Add(BedrijfsMedewerkers medewerker)
        {
            _context.BedrijfsMedewerkers.Add(medewerker);
        }

        public void Delete(int medewerkerId)
        {
            var medewerker = _context.BedrijfsMedewerkers.Find(medewerkerId);
            if (medewerker != null)
            {
                _context.BedrijfsMedewerkers.Remove(medewerker);
            }
        }

        public BedrijfsMedewerkers GetMedewerkerById(int medewerkerId)
        {
            return _context.BedrijfsMedewerkers.Find(medewerkerId);
        }

        public IEnumerable<BedrijfsMedewerkers> GetAll()
        {
            return _context.BedrijfsMedewerkers.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}

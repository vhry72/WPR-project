using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class HuurderRegistratieRepository : IHuurderRegistratieRepository
    {
        private readonly GegevensContext _context;

        public HuurderRegistratieRepository(GegevensContext context)
        {
            _context = context;
        }

        public void Add(ParticulierHuurder particulierHuurder)
        {
            _context.ParticulierHuurders.Add(particulierHuurder);
        }

        public void Delete(int id)
        {
            var particulierHuurder = _context.ParticulierHuurders.Find(id);
            if (particulierHuurder!= null)
            {
                _context.ParticulierHuurders.Remove(particulierHuurder);
            }
        }

        public IEnumerable<ParticulierHuurder> GetAll()
        {
            throw new NotImplementedException();
        }

        public ParticulierHuurder GetById(int id)
        {
            return _context.ParticulierHuurders.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(ParticulierHuurder particulierHuurder)
        {
            _context.ParticulierHuurders.Update(particulierHuurder);
        }
    }
}

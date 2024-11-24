using Microsoft.EntityFrameworkCore;
using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class HuurderRegistratieRepository : IHuurderRegistratieRepository
    {
        private readonly GebruikerGegevensContext _context;

        public HuurderRegistratieRepository(GebruikerGegevensContext context)
        {
            _context = context;
        }

        public void Add(ParticulierHuurder particulierHuurder)
        {
            _context.ParticulierHuurder.Add(particulierHuurder);
        }

        public void Delete(int id)
        {
            var particulierHuurder = _context.ParticulierHuurder.Find(id);
            if (particulierHuurder!= null)
            {
                _context.ParticulierHuurder.Remove(particulierHuurder);
            }
        }

        public IEnumerable<ParticulierHuurder> GetAll()
        {
            throw new NotImplementedException();
        }

        public ParticulierHuurder GetById(int id)
        {
            return _context.ParticulierHuurder.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(ParticulierHuurder particulierHuurder)
        {
            _context.ParticulierHuurder.Update(particulierHuurder);
        }
    }
}

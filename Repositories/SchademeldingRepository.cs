using Microsoft.EntityFrameworkCore;
using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class SchademeldingRepository : ISchademeldingRepository
    {
        private readonly GegevensContext _context;

        public SchademeldingRepository(GegevensContext context)
        {
            _context = context;
        }

        public IQueryable<Schademelding> GetAllSchademeldingen()
        {
            return _context.Schademeldingen
                .Include(s => s.Voertuig);
        }

        public Schademelding GetSchademeldingById(Guid id) 
        {
            return _context.Schademeldingen.FirstOrDefault(s => s.SchademeldingId == id);
        }

        public void Add(Schademelding schademelding)
        {
            _context.Schademeldingen.Add(schademelding);
        }

        public void Update(Schademelding schademelding)
        {
            _context.Schademeldingen.Update(schademelding);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}

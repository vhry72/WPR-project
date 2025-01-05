using Microsoft.EntityFrameworkCore;
using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class VoertuigStatusRepository : IVoertuigStatusRepository
    {
        private readonly GegevensContext _context;

        public VoertuigStatusRepository(GegevensContext context)
        {
            _context = context;
        }

        public IQueryable<VoertuigStatus> GetAllVoertuigenStatussen()
        {
            return _context.VoertuigStatussen
                .Include(h => h.voertuig);
        }
        public VoertuigStatus GetByID(Guid id)

        {
            return _context.VoertuigStatussen.Find(id);
        }
        public void Update(VoertuigStatus voertuigStatus)
        {
            _context.VoertuigStatussen.Update(voertuigStatus);
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

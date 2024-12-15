using WPR_project.Models;
using WPR_project.Data;

namespace WPR_project.Repositories
{
    public class HuurVerzoekRepository : IHuurVerzoekRepository
    {
       
            private readonly GegevensContext _context;

            public HuurVerzoekRepository(GegevensContext context)
            {
                _context = context;
            }

            public IEnumerable<HuurVerzoek> GetAllHuurVerzoeken()
            {
                return _context.Huurverzoeken.ToList();
            }
            public HuurVerzoek GetByID(Guid id)
            {
            return _context.Huurverzoeken.Find(id);
            }
            public void Update(HuurVerzoek huurVerzoek)
            {
            _context.Huurverzoeken.Update(huurVerzoek);
            }

    }

    }


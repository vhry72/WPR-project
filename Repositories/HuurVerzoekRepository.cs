using WPR_project.Data;
using WPR_project.Models;
using WPR_project.Repositories;

public class HuurVerzoekRepository : IHuurVerzoekRepository
{
    private readonly GegevensContext _context;

    // Constructor voor dependency injection
    public HuurVerzoekRepository(GegevensContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(Huurverzoek huurverzoek)
    {
        _context.Huurverzoeken.Add(huurverzoek);
        _context.SaveChanges(); // Zorg ervoor dat wijzigingen worden opgeslagen
    }

    public IEnumerable<Huurverzoek> GetActiveHuurverzoekenByHuurderId(Guid huurderId)
    {
        return _context.Huurverzoeken
            .Where(h => h.HuurderID == huurderId && h.endDate > DateTime.Now)
            .ToList();
    }

    public IEnumerable<Huurverzoek> GetHuurverzoekenForReminder(DateTime reminderTime)
    {
        return _context.Huurverzoeken
            .Where(h => h.beginDate <= reminderTime && h.beginDate > DateTime.Now)
            .ToList();
      }

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



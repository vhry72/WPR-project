using Microsoft.EntityFrameworkCore;
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

    public void Add(Huurverzoek huurVerzoek)
    {
        _context.Huurverzoeken.Add(huurVerzoek);
        _context.SaveChanges();
    }

    public IQueryable<Huurverzoek> GetActiveHuurverzoekenByHuurderId(Guid huurderId)
    {
        return _context.Huurverzoeken
           .Where(h => h.HuurderID == huurderId && h.endDate > DateTime.Now)
           .Include(h => h.Voertuig);
    }
    public IQueryable<Huurverzoek> GetBeantwoordeHuurverzoekenByHuurderId(Guid huurderId)
    {
        return _context.Huurverzoeken
           .Where(h => h.HuurderID == huurderId && (h.isBevestigd == true))
           .Include(h => h.Voertuig);
    }

    public IQueryable<Huurverzoek> GetHuurverzoekenForReminder(DateTime reminderTime)
    {
        return _context.Huurverzoeken
            .Where(h => h.beginDate <= reminderTime && h.beginDate > DateTime.Now)
            .Include(h => h.Voertuig);
    }


    public IQueryable<Huurverzoek> GetAllHuurVerzoeken()
    {
        return _context.Huurverzoeken
    .Include(h => h.Voertuig);

    }
    public IQueryable<Huurverzoek> GetAllActiveHuurVerzoeken()
    {
        return _context.Huurverzoeken
            .Where(h => h.isBevestigd== false)
            .Where(h => h.isBevestigd == false) 

            .Include(h => h.Voertuig);
    }
    public IQueryable<Huurverzoek> GetAllBeantwoorde()
    {
        return _context.Huurverzoeken
            .Where(h => h.isBevestigd == true)
            .Include(h => h.Voertuig);
    }
    public IQueryable<Huurverzoek> GetAllGoedGekeurde()
    {
        return _context.Huurverzoeken
            .Where(h => h.isBevestigd == true && h.approved == true)
            .Include(h => h.Voertuig);
    }
    public IQueryable<Huurverzoek> GetAllAfgekeurde()
    {
        return _context.Huurverzoeken
            .Where(h => (h.isBevestigd == true) && (h.approved == false))
            .Include(h => h.Voertuig);
    }

    public Huurverzoek GetByID(Guid id)

    {
        return _context.Huurverzoeken.Find(id);
    }

    public void Update(Huurverzoek huurVerzoek)
    {
        _context.Huurverzoeken.Update(huurVerzoek);
    }


    public void Save()
    {
        _context.SaveChanges();
    }
}







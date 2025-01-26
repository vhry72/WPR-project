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

    public IQueryable<Voertuig> GetAvailableVehicles(DateTime startDatum, DateTime eindDatum)
    {

        TimeSpan buffer = TimeSpan.FromHours(3);
        // Voeg buffer toe aan de begin- en eindtijden van de huurperiodes om te controleren
        var bufferStartDatum = startDatum.Add(-buffer);
        var bufferEindDatum = eindDatum.Add(buffer);

        // Alle voertuigen ophalen die in een huurverzoek met overlappende datums (inclusief buffer) zijn opgenomen
        var unavailableVehicleIds = _context.Huurverzoeken
            .Where(hv => hv.beginDate < bufferEindDatum && hv.endDate > bufferStartDatum)
            .Select(hv => hv.VoertuigId)
            .Distinct();

        return _context.Voertuigen
            .Where(v => !unavailableVehicleIds.Contains(v.voertuigId));
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

    public List<Huurverzoek> GetHuurverzoekenByHuurderID(Guid huurderId)
    {
        return _context.Huurverzoeken
            .Where(h => h.HuurderID == huurderId)
            .Distinct()
            .ToList();
            
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







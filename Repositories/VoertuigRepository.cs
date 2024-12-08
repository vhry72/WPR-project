using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class VoertuigRepository : IVoertuigRepository
    {
        private readonly GegevensContext _context;

        public VoertuigRepository(GegevensContext context)
        {
            _context = context;
        }

        public IEnumerable<Voertuig> GetAvailableVoertuigen(string voertuigType = null)
        {
            throw new NotImplementedException();
        }

        public Voertuig GetFilteredVoertuigById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Voertuig> GetFilteredVoertuigen(string voertuigType, DateTime? startDatum, DateTime? eindDatum, string sorteerOptie)
        {
            var query = _context.Voertuigen.AsQueryable();

            // Filter op voertuigtype
            if (!string.IsNullOrEmpty(voertuigType))
            {
                query = query.Where(v => v.voertuigType.Equals(voertuigType, StringComparison.OrdinalIgnoreCase));
            }

            // Filter op beschikbaarheid (voorbeeld: niet-gehuurde voertuigen in de geselecteerde periode)
            if (startDatum.HasValue && eindDatum.HasValue)
            {
                // Beschikbaarheid filteren; dit vereist een relatie met een huurmodel
                query = query.Where(v => !_context.Reserveringen.Any(r =>
                    r.VoertuigId == v.voertuigId &&
                    ((r.StartDatum <= eindDatum && r.EindDatum >= startDatum))));
            }

            // Sorteren
            query = sorteerOptie?.ToLower() switch
            {
                "prijs" => query.OrderBy(v => v.prijsPerDag),
                "merk" => query.OrderBy(v => v.merk),
                "beschikbaarheid" => query, // Beschikbaarheid vereist complexere logica
                _ => query
            };

            return query.ToList();
        }

        public Voertuig GetVoertuigById(int id)
        {
            return _context.Voertuigen.FirstOrDefault(v => v.voertuigId == id);
        }
    }
}

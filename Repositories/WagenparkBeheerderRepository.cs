using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class WagenparkBeheerderRepository : IWagenparkBeheerderRepository
    {
        private readonly GegevensContext _context;

        public WagenparkBeheerderRepository(GegevensContext context)
        {
            _context = context;
        }

        public void AddWagenparkBeheerder(WagenparkBeheerder wagenparkBeheerder)
        {
            _context.WagenparkBeheerders.Add(wagenparkBeheerder);
        }

        public void DeleteWagenparkBeheerder(Guid id)
        {
            var wagenparkBeheerder = _context.WagenparkBeheerders.Find(id);
            if (wagenparkBeheerder != null)
            {
                _context.WagenparkBeheerders.Remove(wagenparkBeheerder);
            }
        }

        public Guid GetZakelijkeId(Guid id)
        {
            var zakelijkeId = _context.WagenparkBeheerders
                .Where(m => m.beheerderId == id)
                .Select(m => m.zakelijkeId)
                .FirstOrDefault();

            return zakelijkeId != default ? zakelijkeId : Guid.Empty;
        }



        public IEnumerable<WagenparkBeheerder> GetWagenparkBeheerders()
        {
            return _context.WagenparkBeheerders.ToList();
        }

        public WagenparkBeheerder GetBeheerderById(Guid id)
        {
            return _context.WagenparkBeheerders.Find(id);
        }

        public void UpdateWagenparkBeheerder(WagenparkBeheerder wagenparkBeheerder)
        {
            var existingBeheerder = _context.WagenparkBeheerders.Find(wagenparkBeheerder.beheerderId);
            if (existingBeheerder != null)
            {
                _context.Entry(existingBeheerder).CurrentValues.SetValues(wagenparkBeheerder);
            }
        }

        public List<Guid> GetMedewerkersIdsByWagenparkbeheerder(Guid wagenparkbeheerderId)
        {
            // Haal alleen de IDs van de medewerkers op die gekoppeld zijn aan het gegeven wagenparkbeheerderId
            var medewerkerIds = _context.BedrijfsMedewerkers
                .Where(m => m.beheerderId == wagenparkbeheerderId) // Let op de correcte propertynaam
                .Select(m => m.bedrijfsMedewerkerId)
                .ToList();

            return medewerkerIds;
        }



        public void Save()
        {
            _context.SaveChanges();
        }

        public WagenparkBeheerder getBeheerderById(Guid id)
        {
            throw new NotImplementedException();
        }

        // de 2de updatewagenparkbeheerder is nodig voor de interface om te implementeren, vragen voor hulp zaterdag!
        public void UpdateWagenparkBeheerder(WagenparkBeheerder wagenParkBeheerder, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

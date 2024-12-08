using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class ZakelijkeHuurderRepository : IZakelijkeHuurderRepository
    {
        private readonly GegevensContext _context;

        public ZakelijkeHuurderRepository(GegevensContext context)
        {
            _context = context;
        }

        // Voeg een nieuwe zakelijke huurder toe
        public void AddZakelijkHuurder(ZakelijkHuurder zakelijkHuurder)
        {
            _context.ZakelijkHuurders.Add(zakelijkHuurder);
        }

        // Verwijder een zakelijke huurder via ID
        public void DeleteZakelijkHuurder(Guid id)
        {
            var zakelijkeHuurder = GetZakelijkHuurderById(id);
            if (zakelijkeHuurder != null)
            {
                _context.ZakelijkHuurders.Remove(zakelijkeHuurder);
            }
        }

        // Haal alle zakelijke huurders op
        public IEnumerable<ZakelijkHuurder> GetAllZakelijkHuurders()
        {
            return _context.ZakelijkHuurders.ToList();
        }

        // Haal een specifieke zakelijke huurder op via ID
        public ZakelijkHuurder GetZakelijkHuurderById(Guid id)
        {
            return _context.ZakelijkHuurders.FirstOrDefault(h => h.zakelijkeId == id);
        }

        // Haal een zakelijke huurder op via verificatietoken
        public ZakelijkHuurder GetZakelijkHuurderByToken(string token)
        {
            return _context.ZakelijkHuurders.FirstOrDefault(h => h.EmailBevestigingToken == token);
        }

        // Werk een bestaande zakelijke huurder bij
        public void UpdateZakelijkHuurder(ZakelijkHuurder zakelijkHuurder)
        {
            _context.ZakelijkHuurders.Update(zakelijkHuurder);
        }

        // Sla wijzigingen in de database op
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class ZakelijkeHuurderRepository : IZakelijkeHuurderRepository
    {
        private readonly GegevensContext _context;
        private readonly ILogger<ZakelijkeHuurderRepository> _logger;

        public ZakelijkeHuurderRepository(GegevensContext context , ILogger<ZakelijkeHuurderRepository> logger)
        {
            _context = context;
            _logger = logger;
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

        public Guid GetAbonnementIdByZakelijkeHuurder(Guid id)
        {
            var abonnementId = _context.Abonnementen
                .Where(m => m.zakelijkeId == id)
                .Select(m => m.AbonnementId)
                .FirstOrDefault();

            return abonnementId != default ? abonnementId : Guid.Empty;
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
        public ZakelijkHuurder GetZakelijkHuurderByToken(Guid token)
        {
            if (token == Guid.Empty)
            {
                throw new ArgumentException("Token mag niet leeg zijn.", nameof(token));
            }

            try
            {
                return _context.ZakelijkHuurders.FirstOrDefault(h => h.EmailBevestigingToken == token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van ZakelijkeHuurder met token {Token}.", token);
                throw new Exception("Er is een fout opgetreden bij het ophalen van de zakelijke huurder.");
            }
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

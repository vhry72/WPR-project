using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class PrivacyVerklaringRepository : IPrivacyVerklaringRepository
    {
        private readonly GegevensContext _context;

        public PrivacyVerklaringRepository(GegevensContext context)
        {
            _context = context;
        }

        public void Add(PrivacyVerklaring privacyVerklaring)
        {
            _context.PrivacyVerklaringen.Add(privacyVerklaring);
        }

        public PrivacyVerklaring GetLatestPrivacyVerklaring()
        {
            var latestVerklaring = _context.PrivacyVerklaringen
                                           .OrderByDescending(p => p.UpdateDatum)
                                           .FirstOrDefault();
            if (latestVerklaring == null)
            {
                throw new InvalidOperationException("Er is geen recente PrivacyVerklaring gevonden.");
            }

            return latestVerklaring;
        }

        public IQueryable<PrivacyVerklaring> GetAllPrivacyVerklaringen()
        {
            return _context.PrivacyVerklaringen;
        }
    }
}

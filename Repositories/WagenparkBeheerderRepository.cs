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
            if (wagenparkBeheerder == null)
            {
                throw new InvalidOperationException("Wagenparkbeheerder niet gevonden.");
            }


            var alternatieveBeheerder = _context.WagenparkBeheerders
                .Where(w => w.zakelijkeId == wagenparkBeheerder.zakelijkeId && w.beheerderId != id && w.IsActive)
                .FirstOrDefault();

            if (alternatieveBeheerder != null)
            {

                var medewerkers = _context.BedrijfsMedewerkers
                    .Where(m => m.beheerderId == id);
                foreach (var medewerker in medewerkers)
                {
                    medewerker.beheerderId = alternatieveBeheerder.beheerderId;
                }


                wagenparkBeheerder.IsActive = false;
                var user = _context.Users.FirstOrDefault(u => u.Id == wagenparkBeheerder.AspNetUserId);
                if (user != null)
                {
                    user.IsActive = false;
                }

                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Geen andere actieve wagenparkbeheerders gevonden binnen hetzelfde zakelijkeId. Kan de huidige beheerder niet verwijderen.");
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

        public Guid GetAbonnementId(Guid id)
        {
            var AbonnementId = _context.WagenparkBeheerders
                .Where(m => m.beheerderId == id)
                .Select(m => m.AbonnementId)
                .FirstOrDefault();

            return AbonnementId ?? Guid.Empty;
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

        public List<BedrijfsMedewerkers> GetMedewerkersByWagenparkbeheerder(Guid wagenparkbeheerderId)
        {
            // Haal alleen de IDs van de medewerkers op die gekoppeld zijn aan het gegeven wagenparkbeheerderId
            var medewerkerIds = _context.BedrijfsMedewerkers
                .Where(m => m.beheerderId == wagenparkbeheerderId) // Let op de correcte propertynaam
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

        public void updateWagenparkBeheerderGegevens(WagenparkBeheerder wagenparkbeheerder)
        {
            var emailUpdateIdentity = wagenparkbeheerder.bedrijfsEmail;
            var user = _context.Users.FirstOrDefault(u => u.Id == wagenparkbeheerder.AspNetUserId);
            if (user != null)
            {
                user.Email = emailUpdateIdentity;
                user.UserName = emailUpdateIdentity;
                user.NormalizedEmail = emailUpdateIdentity.ToUpper();
                user.NormalizedUserName = emailUpdateIdentity.ToUpper();
                _context.Users.Update(user);
            }
            _context.WagenparkBeheerders.Update(wagenparkbeheerder);
        }

    }
}

using Hangfire;
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

        public void SetWagenparkBeheerderInactive(Guid id)
        {
            var wagenparkBeheerder = _context.WagenparkBeheerders.Find(id);
            if (wagenparkBeheerder == null)
            {
                throw new InvalidOperationException("Wagenparkbeheerder niet gevonden.");
            }

            // Zoek een alternatieve beheerder
            var alternatieveBeheerder = _context.WagenparkBeheerders
                .Where(w => w.zakelijkeId == wagenparkBeheerder.zakelijkeId && w.beheerderId != id && w.IsActive)
                .FirstOrDefault();

            if (alternatieveBeheerder == null)
            {
                throw new InvalidOperationException("Geen alternatieve actieve wagenparkbeheerders gevonden. Kan de huidige beheerder niet deactiveren.");
            }

            // Reassign medewerkers naar de alternatieve beheerder
            var medewerkers = _context.BedrijfsMedewerkers.Where(m => m.beheerderId == id);
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

            BackgroundJob.Schedule(() => DeleteWagenparkBeheerder(id), TimeSpan.FromDays(730));
        }


        public void DeleteWagenparkBeheerder(Guid id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var wagenparkBeheerder = _context.WagenparkBeheerders.Find(id);
                    if (wagenparkBeheerder == null || !wagenparkBeheerder.IsActive)
                    {
                        throw new InvalidOperationException("Actieve wagenparkbeheerder niet gevonden.");
                    }

                    // Reassign moet al gedaan zijn bij het inactief zetten
                    _context.WagenparkBeheerders.Remove(wagenparkBeheerder);
                    var user = _context.Users.FirstOrDefault(u => u.Id == wagenparkBeheerder.AspNetUserId);
                    if (user != null)
                    {
                        _context.Users.Remove(user);
                    }

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
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

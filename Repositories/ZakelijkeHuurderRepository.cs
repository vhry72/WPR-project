using Hangfire;
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

        public List<WagenparkBeheerder> GetWagenparkBeheerdersByZakelijkeId(Guid id)
        {
            var wagenparkbeheerders = _context.WagenparkBeheerders.Where(z => z.zakelijkeId == id).ToList();

            return wagenparkbeheerders;
        
        
        }

        public void ScheduleDeleteZakelijkHuurder(Guid id)
        {

            DeactivateZakelijkHuurder(id);

            //verwijdering over twee jaar
            BackgroundJob.Schedule(() => DeleteZakelijkHuurderVanDB(id), TimeSpan.FromDays(730));
        }

        public void DeactivateZakelijkHuurder(Guid id)
        {
            var zakelijkHuurder = _context.ZakelijkHuurders.Find(id);

            if (zakelijkHuurder != null)
            {

                zakelijkHuurder.IsActive = false;


                var abonnementen = _context.Abonnementen.Where(a => a.zakelijkeId == id).ToList();
                foreach (var ab in abonnementen)
                {
                    ab.IsActive = false;
                }

                var wagenparkBeheerders = _context.WagenparkBeheerders.Where(w => w.zakelijkeId == id).ToList();
                foreach (var wb in wagenparkBeheerders)
                {
                    wb.IsActive = false;
                    var user = _context.Users.FirstOrDefault(u => u.Id == wb.AspNetUserId);
                    if (user != null)
                    {
                        user.IsActive = false;
                    }
                }

                var bedrijfsMedewerkers = _context.BedrijfsMedewerkers.Where(b => b.zakelijkeId == id).ToList();
                foreach (var bm in bedrijfsMedewerkers)
                {
                    bm.IsActive = false;
                    var user = _context.Users.FirstOrDefault(u => u.Id == bm.AspNetUserId);
                    if (user != null)
                    {
                        user.IsActive = false;
                    }
                }

                var zakelijkHuurderUser = _context.Users.FirstOrDefault(u => u.Id == zakelijkHuurder.AspNetUserId);
                if (zakelijkHuurderUser != null)
                {
                    zakelijkHuurderUser.IsActive = false;
                }


                _context.SaveChanges();
            }
        }

        public void DeleteZakelijkHuurderVanDB(Guid id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var zakelijkHuurder = _context.ZakelijkHuurders.Find(id);

                    if (zakelijkHuurder != null)
                    {
                       
                        var abonnementen = _context.Abonnementen.Where(a => a.zakelijkeId == id);
                        _context.Abonnementen.RemoveRange(abonnementen);

                        
                        var wagenparkBeheerders = _context.WagenparkBeheerders.Where(w => w.zakelijkeId == id);
                        foreach (var wb in wagenparkBeheerders)
                        {
                            var user = _context.Users.FirstOrDefault(u => u.Id == wb.AspNetUserId);
                            if (user != null)
                            {
                                _context.Users.Remove(user);
                            }
                        }
                        _context.WagenparkBeheerders.RemoveRange(wagenparkBeheerders);

                        
                        var bedrijfsMedewerkers = _context.BedrijfsMedewerkers.Where(b => b.zakelijkeId == id);
                        foreach (var bm in bedrijfsMedewerkers)
                        {
                            var user = _context.Users.FirstOrDefault(u => u.Id == bm.AspNetUserId);
                            if (user != null)
                            {
                                _context.Users.Remove(user);
                            }
                        }
                        _context.BedrijfsMedewerkers.RemoveRange(bedrijfsMedewerkers);

                        
                        var zakelijkHuurderUser = _context.Users.FirstOrDefault(u => u.Id == zakelijkHuurder.AspNetUserId);
                        if (zakelijkHuurderUser != null)
                        {
                            _context.Users.Remove(zakelijkHuurderUser);
                        }

                        
                        _context.ZakelijkHuurders.Remove(zakelijkHuurder);

                        
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    
                    transaction.Rollback();
                    throw new InvalidOperationException("Het is niet gelukt om de bedrijf te verwijderen"); ;
                }
            }
        }


        public Guid? GetAbonnementIdByZakelijkeHuurder(Guid id)
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
            var emailUpdateIdentity = zakelijkHuurder.bedrijfsEmail;
            var user = _context.Users.FirstOrDefault(u => u.Id == zakelijkHuurder.AspNetUserId);
            if (user != null)
            {
                user.Email = emailUpdateIdentity;
                user.UserName = emailUpdateIdentity;
                user.NormalizedEmail = emailUpdateIdentity.ToUpper();
                user.NormalizedUserName = emailUpdateIdentity.ToUpper();
                _context.Users.Update(user);
            }

            
            _context.ZakelijkHuurders.Update(zakelijkHuurder);


            var beheerders = _context.WagenparkBeheerders.Where(w => w.zakelijkeId == zakelijkHuurder.zakelijkeId).ToList();
            foreach (var beheerder in beheerders)
            {
                beheerder.Adres = zakelijkHuurder.adres;
                beheerder.telefoonNummer = zakelijkHuurder.telNummer;
                beheerder.KVKNummer = zakelijkHuurder.KVKNummer;
                _context.WagenparkBeheerders.Update(beheerder);
            }

            _context.SaveChanges();
        }


        // Sla wijzigingen in de database op
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

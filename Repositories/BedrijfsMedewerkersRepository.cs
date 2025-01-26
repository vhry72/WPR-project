using Hangfire;
using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class BedrijfsMedewerkersRepository : IBedrijfsMedewerkersRepository
    {
        private readonly GegevensContext _context;

        public BedrijfsMedewerkersRepository(GegevensContext context)
        {
            _context = context;
        }
        public void Deactivate(Guid medewerkerId)
        {
            var bedrijfsMedewerker = _context.BedrijfsMedewerkers.Find(medewerkerId);
            if (bedrijfsMedewerker != null)
            {

                bedrijfsMedewerker.IsActive = false;

                var user = _context.Users.FirstOrDefault(u => u.Id == bedrijfsMedewerker.AspNetUserId);

                if (user != null)
                {

                    user.IsActive = false;
                }

                _context.SaveChanges();

                BackgroundJob.Schedule(() => Delete(medewerkerId), TimeSpan.FromDays(370));
            }
        }


        public void Delete(Guid medewerkerId)
        {
            var bedrijfsMedewerker = _context.BedrijfsMedewerkers.Find(medewerkerId);
            if (bedrijfsMedewerker != null)
            {

                bedrijfsMedewerker.IsActive = false;

                var user = _context.Users.FirstOrDefault(u => u.Id == bedrijfsMedewerker.AspNetUserId);

                if (user != null)
                {

                    _context.Remove(user);
                }

                _context.BedrijfsMedewerkers.Remove(bedrijfsMedewerker);
                _context.SaveChanges();
            }
        }

        public BedrijfsMedewerkers GetMedewerkerById(Guid medewerkerId)
        {
            return _context.BedrijfsMedewerkers.Find(medewerkerId);
        }

        public void Update(BedrijfsMedewerkers bedrijfsMedewerkers)
        {
            var emailUpdateIdentity = bedrijfsMedewerkers.medewerkerEmail;
            var user = _context.Users.FirstOrDefault(u => u.Id == bedrijfsMedewerkers.AspNetUserId);
            if (user != null)
            {
                user.Email = emailUpdateIdentity;
                user.UserName = emailUpdateIdentity;
                user.NormalizedEmail = emailUpdateIdentity.ToUpper();
                user.NormalizedUserName = emailUpdateIdentity.ToUpper();
                _context.Users.Update(user);
            }
            _context.BedrijfsMedewerkers.Update(bedrijfsMedewerkers);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
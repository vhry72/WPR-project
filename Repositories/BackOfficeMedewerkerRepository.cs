using Hangfire;
using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class BackOfficeMedewerkerRepository : IBackOfficeMedewerkerRepository
    {
        private readonly GegevensContext _context;

        public BackOfficeMedewerkerRepository(GegevensContext context)      
        {
            _context = context;
        }

        public BackofficeMedewerker GetBackofficemedewerkerById(Guid id)
        {
            return _context.BackofficeMedewerkers.Find(id);
        }

        public void AddBackOfficeMedewerker(BackofficeMedewerker backOfficeMedewerker)
        {
            _context.BackofficeMedewerkers.Add(backOfficeMedewerker);
        }

        public void UpdateBackOfficeMedewerker(BackofficeMedewerker backOfficeMedewerker)
        {
            var emailUpdateIdentity = backOfficeMedewerker.medewerkerEmail;
            var user = _context.Users.FirstOrDefault(u => u.Id == backOfficeMedewerker.AspNetUserId);
            if (user != null)
            {
                user.Email = emailUpdateIdentity;
                user.UserName = emailUpdateIdentity;
                user.NormalizedEmail = emailUpdateIdentity.ToUpper();
                user.NormalizedUserName = emailUpdateIdentity.ToUpper();
                _context.Users.Update(user);
            }
            _context.BackofficeMedewerkers.Update(backOfficeMedewerker);
        }

        public void DeactivateBackOfficeMedewerker(Guid id)
        {
            var backOfficeMedewerker = _context.BackofficeMedewerkers.Find(id);
            if (backOfficeMedewerker != null)
            {

                backOfficeMedewerker.IsActive = false;

                var user = _context.Users.FirstOrDefault(u => u.Id == backOfficeMedewerker.AspNetUserId);

                if (user != null)
                {

                    user.IsActive = false;
                }

                _context.SaveChanges();

                BackgroundJob.Schedule(() => DeleteBackOfficeMedewerker(id), TimeSpan.FromDays(365));
            }
        }


        public void DeleteBackOfficeMedewerker(Guid id)
        {
            var backOfficeMedewerker = _context.BackofficeMedewerkers.Find(id);
            if (backOfficeMedewerker != null)
            {

                backOfficeMedewerker.IsActive = false;

                var user = _context.Users.FirstOrDefault(u => u.Id == backOfficeMedewerker.AspNetUserId);

                if (user != null)
                {

                   _context.Users.Remove(user);
                }

                _context.BackofficeMedewerkers.Remove(backOfficeMedewerker);
                _context.SaveChanges();

            }
        }



        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

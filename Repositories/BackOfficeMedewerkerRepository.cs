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

        public void UpdateBackOfficeMedewerker(BackofficeMedewerker backOfficeMedewerker, Guid id)
        {
            var backOfficeMedewerkerToUpdate = _context.BackofficeMedewerkers.Find(id);
            if (backOfficeMedewerkerToUpdate != null)
            {
                backOfficeMedewerkerToUpdate.medewerkerNaam = backOfficeMedewerker.medewerkerNaam;
                backOfficeMedewerkerToUpdate.medewerkerEmail = backOfficeMedewerker.medewerkerEmail;
                backOfficeMedewerkerToUpdate.wachtwoord = backOfficeMedewerker.wachtwoord;
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

                    user.IsActive = false;
                }

                _context.SaveChanges();
            }
        }



        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

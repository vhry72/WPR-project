using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddMedewerker(BedrijfsMedewerkers medewerker)
        {
            if (medewerker == null)
                throw new ArgumentNullException(nameof(medewerker));

            _context.BedrijfsMedewerkers.Add(medewerker);
        }

        public void Delete(Guid medewerkerId)
        {
            var medewerker = _context.BedrijfsMedewerkers.Find(medewerkerId);
            if (medewerker != null)
            {
                _context.BedrijfsMedewerkers.Remove(medewerker);
                _context.SaveChanges(); // SaveChanges toegevoegd om wijzigingen door te voeren
            }
        }

        public BedrijfsMedewerkers GetMedewerkerById(Guid medewerkerId)
        {
            return _context.BedrijfsMedewerkers.Find(medewerkerId);
        }

        public IEnumerable<BedrijfsMedewerkers> GetAll()
        {
            return _context.BedrijfsMedewerkers.ToList();
        }
        public BedrijfsMedewerkers GetByEmailAndPassword(string email, string password)
        {
            return _context.BedrijfsMedewerkers
                           .FirstOrDefault(h => h.medewerkerEmail == email && h.wachtwoord == password);
        }

        public void Update(BedrijfsMedewerkers bedrijfsMedewerkers)
        {
            _context.BedrijfsMedewerkers.Update(bedrijfsMedewerkers);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
using WPR_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WPR_project.Data;

namespace WPR_project.Repositories
{
    public class AbonnementRepository : IAbonnementRepository
    {
        private readonly GegevensContext _context;

        public AbonnementRepository(GegevensContext context)
        {
            _context = context;
        }

        public BedrijfsMedewerkers GetMedewerkerById(Guid medewerkerId)
        {
            return _context.BedrijfsMedewerkers.FirstOrDefault(m => m.bedrijfsMedewerkerId == medewerkerId);
        }


        public IEnumerable<Abonnement> GetAllAbonnementen()
        {
            return _context.Abonnementen.ToList();
        }

        public Abonnement GetAbonnementById(Guid id)
        {
            return _context.Abonnementen.Find(id);
        }

        public void AddAbonnement(Abonnement abonnement)
        {
            _context.Abonnementen.Add(abonnement);
        }

        public void UpdateAbonnement(Abonnement abonnement)
        {
            var existingAbonnement = _context.Abonnementen.Find(abonnement.AbonnementId);
            if (existingAbonnement != null)
            {
                _context.Entry(existingAbonnement).CurrentValues.SetValues(abonnement);
            }
        }

        public void DeleteAbonnement(Guid id)
        {
            var abonnement = _context.Abonnementen.Find(id);
            if (abonnement != null)
            {
                _context.Abonnementen.Remove(abonnement);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
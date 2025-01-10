using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class VoertuigRepository : IVoertuigRepository
    {
        private readonly GegevensContext _context;

        public VoertuigRepository(GegevensContext context)
        {
            _context = context;
        }

        public IEnumerable<Voertuig> GetAvailableVoertuigen(string voertuigType = null)
        {
            throw new NotImplementedException();
        }

        public Voertuig GetFilteredVoertuigById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Voertuig> GetFilteredVoertuigen(string voertuigType, DateTime? startDatum, DateTime? eindDatum, string sorteerOptie)
        {
            var query = _context.Voertuigen.AsQueryable();

            // Filter op voertuigtype
            if (!string.IsNullOrEmpty(voertuigType))
            {
                query = query.Where(v => v.voertuigType.Equals(voertuigType));
            }

            return query.ToList();
        }

        public Voertuig GetVoertuigById(Guid id)
        {
            var voertuig = _context.Voertuigen.SingleOrDefault(v => v.voertuigId == id);
            if (voertuig == null)
            {
                throw new KeyNotFoundException($"Voertuig met ID {id} is niet gevonden.");
            }
            return voertuig;
        }
        public Voertuig GetVoertuigByKenteken(string kenteken)
        {
            var voertuig = _context.Voertuigen.FirstOrDefault(v => v.kenteken == kenteken);
            if (voertuig== null)
            {
                throw new KeyNotFoundException($"Voertuig met kenteken: {kenteken} is niet gevonden.");
            }
            return voertuig;
        }

        public IEnumerable<Voertuig> GetVoertuigTypeVoertuigen(string voertuigType)
        {
            var query = _context.Voertuigen.AsQueryable();

            // Filter op voertuigtype
            if (!string.IsNullOrEmpty(voertuigType))
            {
                query = query.Where(v => v.voertuigType.Equals(voertuigType));
            }

            return query.ToList();
        }

        public VoertuigStatus GetVoertuigStatus(Guid voertuigId)
        {
            var voertuig = _context.Voertuigen
                .Include(v => v.voertuigStatus)
                .FirstOrDefault(v => v.voertuigId == voertuigId);

            if (voertuig == null || voertuig.voertuigStatus == null)
            {
                throw new KeyNotFoundException("Voertuig of status niet gevonden.");
            }

            return voertuig.voertuigStatus;
        }
        public Voertuig GetByID(Guid id)

        {
            return _context.Voertuigen.Find(id);
        }
        public IQueryable<Voertuig> GetAllVoertuigen()
        {
            return _context.Voertuigen
            .Include(h => h.voertuigStatus);

        }

        public void updateVoertuig(Voertuig voertuig)
        {
            _context.Voertuigen.Update(voertuig);
            _context.SaveChanges();
        }
        public void Delete(Guid id)
        {
            var voertuig = _context.Voertuigen.Find(id);
            if (voertuig != null)
            {
                _context.Voertuigen.Remove(voertuig);
            }
        }
        public void Add(Voertuig voertuig)
        {
            _context.Voertuigen.Add(voertuig);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}

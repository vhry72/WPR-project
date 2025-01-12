using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPR_project.Data;
using WPR_project.Models;
using WPR_project.DTO_s;

namespace WPR_project.Repositories
{
    public class VoertuigNotitiesRepository : IVoertuigNotitiesRepository
    {
        private readonly GegevensContext _context;

        public VoertuigNotitiesRepository(GegevensContext context)
        {
            _context = context;

        }
        public IEnumerable<VoertuigNotities> GetVoertuigNotitiesByVoertuigId(Guid voertuigId)
        {
            return _context.VoertuigNotities.Where(v => v.voertuigId == voertuigId).ToList();
        }

        public void AddVoertuigNotitie(VoertuigNotities voertuigNotities)
        {
            _context.VoertuigNotities.Add(voertuigNotities);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

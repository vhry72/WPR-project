﻿using Microsoft.EntityFrameworkCore;
using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class FrontOfficeMedewerkerRepository : IFrontOfficeMedewerkerRepository
    {
        private readonly GegevensContext _context;

        public FrontOfficeMedewerkerRepository(GegevensContext context)
        {
            _context = context;
        }
        public FrontofficeMedewerker GetFrontOfficeMedewerkerById(Guid id)
        {
            return _context.FrontofficeMedewerkers.Find(id);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Update(FrontofficeMedewerker frontofficeMedewerker)
        {
            _context.FrontofficeMedewerkers.Update(frontofficeMedewerker);
        }
    }
}

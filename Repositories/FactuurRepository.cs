using System;
using WPR_project.Models;  // Veronderstelt dat je een model hebt voor facturen
using Microsoft.EntityFrameworkCore;
using WPR_project.Data;

namespace WPR_project.Repositories
{
    public class FactuurRepository : IFactuurRepository
    {
        private readonly GegevensContext _context;  // Veronderstelt dat je een DbContext klasse hebt voor EF Core

        public FactuurRepository(GegevensContext context)
        {
            _context = context;
        }

        public void SaveInvoice(Factuur factuur)
        {
            _context.Facturen.Add(factuur);
            _context.SaveChanges();
        }

        public Factuur GetInvoiceById(Guid invoiceId)
        {
            return _context.Facturen.Find(invoiceId);
        }
    }
}

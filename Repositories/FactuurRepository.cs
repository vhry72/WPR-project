using WPR_project.Data;

namespace WPR_project.Repositories
{
    public class FactuurRepository : IFactuurRepository
    {
        private readonly GegevensContext _context; 

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

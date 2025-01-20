namespace WPR_project.Repositories
{
    public interface IFactuurRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        void SaveInvoice(Factuur factuur);
        Factuur GetInvoiceById(Guid invoiceId);
    }
}

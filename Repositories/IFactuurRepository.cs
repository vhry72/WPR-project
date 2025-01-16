namespace WPR_project.Repositories
{
    public interface IFactuurRepository
    {
        void SaveInvoice(Factuur factuur);
        Factuur GetInvoiceById(Guid invoiceId);
    }
}

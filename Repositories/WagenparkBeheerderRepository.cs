using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class WagenparkBeheerderRepository : IWagenparkBeheerderRepository
    {
        private readonly GegevensContext _context;

        public WagenparkBeheerderRepository(GegevensContext context)
        {
            _context = context;
        }

        public void AddWagenparkBeheerder(WagenparkBeheerder wagenparkBeheerder)
        {
            _context.WagenparkBeheerders.Add(wagenparkBeheerder);
        }

        public void DeleteWagenparkBeheerder(Guid id)
        {
            var wagenparkBeheerder = _context.WagenparkBeheerders.Find(id);
            if (wagenparkBeheerder != null)
            {
                _context.WagenparkBeheerders.Remove(wagenparkBeheerder);
            }
        }

        public IEnumerable<WagenparkBeheerder> GetWagenparkBeheerders()
        {
            return _context.WagenparkBeheerders.ToList();
        }

        public WagenparkBeheerder GetBeheerderById(Guid id)
        {
            return _context.WagenparkBeheerders.Find(id);
        }

        public void UpdateWagenparkBeheerder(WagenparkBeheerder wagenparkBeheerder)
        {
            var existingBeheerder = _context.WagenparkBeheerders.Find(wagenparkBeheerder.beheerderId);
            if (existingBeheerder != null)
            {
                _context.Entry(existingBeheerder).CurrentValues.SetValues(wagenparkBeheerder);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public WagenparkBeheerder getBeheerderById(Guid id)
        {
            throw new NotImplementedException();
        }

        // de 2de updatewagenparkbeheerder is nodig voor de interface om te implementeren, vragen voor hulp zaterdag!
        public void UpdateWagenparkBeheerder(WagenparkBeheerder wagenParkBeheerder, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

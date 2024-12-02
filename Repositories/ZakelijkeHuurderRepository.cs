using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public class ZakelijkeHuurderRepository : IZakelijkeHuurderRepository
    {
        private readonly GegevensContext _context;

        public ZakelijkeHuurderRepository(GegevensContext context)
        {
            _context = context;
        }

        public void AddZakelijkHuurder(ZakelijkHuurder zakelijkHuurder)
        {
            _context.ZakelijkHuurders.Add(zakelijkHuurder);
        }

        public void DeleteZakelijkHuurder(int id)
        {
            var zakelijkeHuurder = _context.ZakelijkHuurders.Find(id);
            if (zakelijkeHuurder != null)
            {
                _context.ZakelijkHuurders.Remove(zakelijkeHuurder);
            }
        }

        public IEnumerable<ZakelijkHuurder> GetAllZakelijkHuurders()
        {
            return _context.ZakelijkHuurders.ToList();
        }

        public ZakelijkHuurder GetZakelijkHuurderById(int id)
        {
            return _context.ZakelijkHuurders.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateZakelijkHuurder(ZakelijkHuurder zakelijkHuurder)
        {
            _context.ZakelijkHuurders.Update(zakelijkHuurder);
        }
    }
}

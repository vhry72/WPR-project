    using WPR_project.Data;
    using WPR_project.Models;

    namespace WPR_project.Repositories
    {
        public class HuurderRegistratieRepository : IHuurderRegistratieRepository
        {
            private readonly GegevensContext _context;

            public HuurderRegistratieRepository(GegevensContext context)
            {
                _context = context;
            }

            public void Add(ParticulierHuurder particulierHuurder)
            {
                _context.ParticulierHuurders.Add(particulierHuurder);
            }

            public void Delete(int id)
            {
                var particulierHuurder = _context.ParticulierHuurders.Find(id);
                if (particulierHuurder != null)
                {
                    _context.ParticulierHuurders.Remove(particulierHuurder);
                }
            }

            public void Delete(ParticulierHuurder huurder)
            {
                _context.ParticulierHuurders.Remove(huurder);
            }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ParticulierHuurder> GetAll()
            {
                return _context.ParticulierHuurders.ToList();
            }

            public ParticulierHuurder GetById(Guid id)
            {
                return _context.ParticulierHuurders.Find(id);
            }

            public ParticulierHuurder GetByToken(string token)
            {
                return _context.ParticulierHuurders.FirstOrDefault(h => h.EmailBevestigingToken == token);
            }

            public void Save()
            {
                _context.SaveChanges();
            }

            public void Update(ParticulierHuurder particulierHuurder)
            {
                _context.ParticulierHuurders.Update(particulierHuurder);
            }
        }
    }
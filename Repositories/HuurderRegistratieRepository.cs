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

        public void Delete(Guid id)
        {
            var particulierHuurder = _context.ParticulierHuurders.Find(id);
            if (particulierHuurder != null)
            {

                particulierHuurder.IsActive = false;

                var user = _context.Users.FirstOrDefault(u => u.Id == particulierHuurder.AspNetUserId);

                if (user != null)
                {

                    user.IsActive = false;
                }

                _context.SaveChanges();
            }
        }

        public IEnumerable<ParticulierHuurder> GetAll()
            {
                return _context.ParticulierHuurders.ToList();
            }

            public ParticulierHuurder GetById(Guid id)
            {
                return _context.ParticulierHuurders.Find(id);
            }

        public ParticulierHuurder GetByEmailAndPassword(string email, string password)
        {
            return _context.ParticulierHuurders
                           .FirstOrDefault(h => h.particulierEmail == email && h.wachtwoord == password);
        }


        public ParticulierHuurder GetByToken(Guid token)
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
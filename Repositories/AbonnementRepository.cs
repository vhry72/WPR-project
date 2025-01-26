using WPR_project.Data;
using WPR_project.DTO_s;
using Hangfire;

namespace WPR_project.Repositories
{
    public class AbonnementRepository : IAbonnementRepository
    {
        private readonly GegevensContext _context;

        public AbonnementRepository(GegevensContext context)
        {
            _context = context;
        }


        public IEnumerable<Abonnement> GetAllAbonnementen()
        {
            return _context.Abonnementen.ToList();
        }


        public Abonnement GetAbonnementById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new InvalidOperationException("De HuurverzoekID is niet geldig");
            }
            var abonnementen = _context.Abonnementen.FirstOrDefault(a => a.AbonnementId == id);

            if (abonnementen == null)
            {
                throw new InvalidOperationException("Het Abonnement kon niet opgehaald worden");
            }
            else
            {
                return abonnementen;
            }
        }

        public void AddAbonnement(Abonnement abonnement)
        {
            _context.Abonnementen.Add(abonnement);
        }

        public void UpdateAbonnement(Abonnement abonnement)
        {
            var existingAbonnement = _context.Abonnementen.Find(abonnement.AbonnementId);
            if (existingAbonnement != null)
            {
                _context.Entry(existingAbonnement).CurrentValues.SetValues(abonnement);
            }
        }

        public void UpdateAbonnementMetHangfire(AbonnementWijzigDTO dto)
        {
            try
            {
                var abonnement = _context.Abonnementen.Find(dto.AbonnementId);
                if (abonnement == null)
                {
                    throw new InvalidOperationException("Het abonnement kon niet gevonden worden.");
                }
                else
                {
                    
                    abonnement.Naam = dto.Naam;
                    abonnement.Kosten = dto.Kosten;
                    abonnement.korting = dto.korting;
                    abonnement.zakelijkeId = dto.zakelijkeId;
                    abonnement.AbonnementType = dto.AbonnementType;
                    abonnement.AbonnementTermijnen = dto.AbonnementTermijnen;

                    _context.Abonnementen.Update(abonnement);
                    Save();
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public void UpdateInToekomst(AbonnementWijzigDTO abonnement)
        {
            if (abonnement.updateDatum == null || abonnement.updateDatum <= DateTime.Now)
            {
                throw new ArgumentException("Update datum moet in de toekomst liggen.");
            }

            
            BackgroundJob.Schedule(() => UpdateAbonnementMetHangfire(abonnement), abonnement.updateDatum.Value - DateTime.Now);
        }



        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class BackOfficeService
    {
        private readonly IBackOfficeMedewerkerRepository _repository;
        private readonly IEmailService _emailService;
        public BackOfficeService(
            IBackOfficeMedewerkerRepository repository,
            IEmailService emailService)
        {

            _repository = repository;
            _emailService = emailService;
        }


        public BackofficeMedewerkerWijzigDTO GetGegevensById(Guid id)
        {
            var huurder = _repository.GetBackofficemedewerkerById(id);
            if (huurder == null) return null;

            return new BackofficeMedewerkerWijzigDTO
            {
                medewerkerEmail = huurder.medewerkerEmail,
                medewerkerNaam = huurder.medewerkerNaam,

            };

        }

        public void Update(Guid id, BackofficeMedewerkerWijzigDTO dto)
        {
            var huurder = _repository.GetBackofficemedewerkerById(id);
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");

            huurder.medewerkerNaam = dto.medewerkerNaam;
            huurder.medewerkerEmail = dto.medewerkerEmail;

            _repository.UpdateBackOfficeMedewerker(huurder);
            _repository.Save();
        }


        public void Delete(Guid id)
        {
            var huurder = _repository.GetBackofficemedewerkerById(id);
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID is verplicht.");
            }
            _repository.DeactivateBackOfficeMedewerker(id);
            string bericht = $"Beste {huurder.medewerkerNaam},\n\n Uw account wordt verwijderd, \n\n Vriendelijke Groet, \n CarAndAll";
            _emailService.SendEmail(huurder.medewerkerEmail, "Account verwijderd", bericht);
        }

        public void KeuringAbonnement(Guid id, bool keuring)
        {
            try
            {
                _repository.AbonnementKeuring(id, keuring);

            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);

            }
        }
    }
}

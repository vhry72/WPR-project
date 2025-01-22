using NuGet.Protocol.Core.Types;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class BackOfficeService
    {
        private readonly IBackOfficeMedewerkerRepository _repository;
        private readonly IFrontOfficeMedewerkerRepository _frontOfficeMedewerkerRepository;
        private readonly IEmailService _emailService;
        public BackOfficeService(
            IBackOfficeMedewerkerRepository repository,
            IFrontOfficeMedewerkerRepository frontOfficeMedewerkerRepository, 
            IEmailService emailService)
        {

            _repository = repository;
            _frontOfficeMedewerkerRepository = frontOfficeMedewerkerRepository;
            _emailService = emailService;
        }

        public void AddBackofficeMedewerker(BackofficeMedewerker backofficeMedewerker)
        {
            _repository.AddBackOfficeMedewerker(backofficeMedewerker);
            _repository.Save();
        }

        public void UpdateBackofficeMedewerker(BackofficeMedewerker backOfficeMedewerker, Guid id)
        {
            var existingMedewerker = _repository.GetBackofficemedewerkerById(id);
            if (existingMedewerker != null)
            {
                existingMedewerker.medewerkerNaam = backOfficeMedewerker.medewerkerNaam;
                existingMedewerker.medewerkerEmail = backOfficeMedewerker.medewerkerEmail;
                existingMedewerker.wachtwoord = backOfficeMedewerker.wachtwoord;

                _repository.UpdateBackOfficeMedewerker(existingMedewerker, id);
                _repository.Save();
            }
            else
            {
                throw new KeyNotFoundException("Backofficemedewerker niet gevonden.");
            }
        }

        public void Delete(Guid id)
        {
            var huurder = _repository.GetBackofficemedewerkerById(id);
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID is verplicht.");
            }
            _repository.DeleteBackOfficeMedewerker(id);
            string bericht = $"Beste {huurder.medewerkerNaam},\n\n Uw account wordt verwijderd, \n\n Vriendelijke Groet, \n CarAndAll";
            _emailService.SendEmail(huurder.medewerkerEmail, "Account verwijderd", bericht);
        }

        public void GetFrontofficeMedewerkerById(Guid id)
        {
            var medewerker = _frontOfficeMedewerkerRepository.GetFrontOfficeMedewerkerById(id);
            if (medewerker == null)
            {
                throw new KeyNotFoundException("Frontofficemedewerker niet gevonden.");
            }
        }
    }

}

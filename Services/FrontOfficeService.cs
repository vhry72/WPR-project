using NuGet.Protocol.Core.Types;
using WPR_project.Data;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class FrontOfficeService
    {
        private readonly IFrontOfficeMedewerkerRepository _frontOfficeMedewerkerRepository;
        private readonly IEmailService _emailService;

        public FrontOfficeService(
            IBackOfficeMedewerkerRepository repository,
            IFrontOfficeMedewerkerRepository frontOfficeMedewerkerRepository,
            GegevensContext context,
            IEmailService emailService)
        {
            _frontOfficeMedewerkerRepository = frontOfficeMedewerkerRepository;
            _emailService = emailService;
        }

        public IQueryable GetAll()
        {
            return _frontOfficeMedewerkerRepository.Getall();
        }

        public void VoegMedewerkerToe(Guid frontOfficeId, string medewerkerNaam, string medewerkerEmail, string wachtwoord)
        {
            var medewerker = _frontOfficeMedewerkerRepository.GetFrontOfficeMedewerkerById(frontOfficeId);
            if (medewerker == null)
                throw new KeyNotFoundException("Front Office medewerker niet gevonden.");

            if (medewerker.FrontofficeMedewerkers.Any(m => m.medewerkerEmail.Equals(medewerkerEmail, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Deze medewerker bestaat al.");

            var frontMedewerker = new FrontofficeMedewerker
            {
                FrontofficeMedewerkerId = Guid.NewGuid(),
                medewerkerNaam = medewerkerNaam,
                medewerkerEmail = medewerkerEmail,
                wachtwoord = wachtwoord
            };

            medewerker.FrontofficeMedewerkers.Add(frontMedewerker);
            _frontOfficeMedewerkerRepository.Update(medewerker);
            _frontOfficeMedewerkerRepository.Save();

            string bericht = $"Beste {medewerkerNaam},\n\nU bent toegevoegd.";
            _emailService.SendEmail(medewerkerEmail, "Welkom bij de CarAndAll familie", bericht);
        }

        public void Delete(Guid id)
        {
            var medewerker = _frontOfficeMedewerkerRepository.GetFrontOfficeMedewerkerById(id);
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID is verplicht.");
            }
            _frontOfficeMedewerkerRepository.Delete(id);
            string bericht = $"Beste {medewerker.medewerkerNaam},\n\n Uw account wordt verwijderd, \n\n Vriendelijke Groet, \n CarAndAll";
            _emailService.SendEmail(medewerker.medewerkerEmail, "Medewerker verwijderd", bericht);
        }

        public FrontofficeMedewerkerWijzigDTO GetGegevensById(Guid id)
        {
            var huurder = _frontOfficeMedewerkerRepository.GetFrontOfficeMedewerkerById(id);
            if (huurder == null) return null;

            return new FrontofficeMedewerkerWijzigDTO
            {
                medewerkerEmail = huurder.medewerkerEmail,
                medewerkerNaam = huurder.medewerkerNaam,

            };

        }

        public void Update(Guid id, FrontofficeMedewerkerWijzigDTO dto)
        {
            var huurder = _frontOfficeMedewerkerRepository.GetFrontOfficeMedewerkerById(id);
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");

            huurder.medewerkerNaam = dto.medewerkerNaam;
            huurder.medewerkerEmail = dto.medewerkerEmail;

            _frontOfficeMedewerkerRepository.Update(huurder);
            _frontOfficeMedewerkerRepository.Save();
        }

    }
}

using WPR_project.Data;
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

        public IQueryable GetAll() {
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

        public void VerwijderMedewerker(Guid frontOfficeMedewerkerId, Guid medewerkerId)
        {
            var frontOfficeMedewerker = _frontOfficeMedewerkerRepository.GetFrontOfficeMedewerkerById(frontOfficeMedewerkerId);
            if (frontOfficeMedewerker == null)
                throw new KeyNotFoundException("Front office medewerker niet gevonden.");

            var medewerkerOmTeVerwijderen = frontOfficeMedewerker.FrontofficeMedewerkers.FirstOrDefault(m => m.FrontofficeMedewerkerId == medewerkerId);
            if (medewerkerOmTeVerwijderen == null)
                throw new KeyNotFoundException("Medewerker niet gevonden.");

            frontOfficeMedewerker.FrontofficeMedewerkers.Remove(medewerkerOmTeVerwijderen);
            _frontOfficeMedewerkerRepository.Update(frontOfficeMedewerker);
            _frontOfficeMedewerkerRepository.Save();

            string bericht = $"Beste {medewerkerOmTeVerwijderen.medewerkerNaam},\n\nU bent verwijderd uit het bedrijf CarAndAll.";
            _emailService.SendEmail(medewerkerOmTeVerwijderen.medewerkerEmail, "Medewerker verwijderd", bericht);
        }
    }
}

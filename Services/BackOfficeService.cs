using WPR_project.Data;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class BackOfficeService
    {
        private readonly IBackOfficeMedewerkerRepository backOfficeMedewerkerRepository;
        private readonly IFrontOfficeMedewerkerRepository frontOfficeMedewerkerRepository;
        private readonly IEmailService _emailService;
        private readonly GegevensContext _context;

        public BackOfficeService(
            IBackOfficeMedewerkerRepository repository,
            IFrontOfficeMedewerkerRepository frontOfficeMedewerkerRepository,
            GegevensContext context,
            IEmailService emailService)
        {
            backOfficeMedewerkerRepository = repository;
            frontOfficeMedewerkerRepository = frontOfficeMedewerkerRepository;
            _context = context;
            _emailService = emailService;
        }
        public void VoegMedewerkerToe(Guid frontOfficeId, string medewerkerNaam, string medewerkerEmail, string wachtwoord)
        {
            // Controleer of de huurder bestaat
            var medewerker = frontOfficeMedewerkerRepository.GetFrontOfficeMedewerkerById(frontOfficeId);
            if (medewerker == null)
                throw new KeyNotFoundException("Front Office medewerker niet gevonden.");

            // Controleer of de medewerker al bestaat
            if (medewerker.FrontofficeMedewerkers.Any(m => m.medewerkerEmail.Equals(medewerkerEmail, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Deze medewerker bestaat al.");

            // Maak een nieuwe medewerker aan
            var frontMedewerker = new FrontofficeMedewerker
            {
                FrontofficeMedewerkerId = Guid.NewGuid(),
                medewerkerNaam = medewerkerNaam,
                medewerkerEmail = medewerkerEmail,
                wachtwoord = wachtwoord, // Sla dit veilig op (versleutel bijvoorbeeld)

            };
            // Voeg de medewerker toe aan de frontofficemedewerkers
            medewerker.FrontofficeMedewerkers.Add(medewerker);

            // Update de medewerker in de repository
            frontOfficeMedewerkerRepository.Update(medewerker);
            frontOfficeMedewerkerRepository.Save();

            // Verstuur een e-mail naar de nieuwe medewerker
            string bericht = $"Beste {medewerkerNaam},\n\nU bent toegevoegd.";
            _emailService.SendEmail(medewerkerEmail, "Welkom bij de CarAndAll familie", bericht);
        }
        public BackofficeMedewerker? GetMedewerkerById(Guid medewerkerId)
        {
            return _context.BackofficeMedewerkers.FirstOrDefault(m => m.BackofficeMedewerkerId == medewerkerId);
        }
        public void VerwijderMedewerker(Guid frontOfficeMedewerkerId, Guid medewerkerId)
        {
            var medewerker = frontOfficeMedewerkerRepository.GetFrontOfficeMedewerkerById(medewerkerId);
            if (medewerker == null)
                throw new KeyNotFoundException("Front office medewerker niet gevonden.");

            var backoffice = medewerker.FrontofficeMedewerkers.FirstOrDefault(m => m.FrontofficeMedewerkerId== medewerkerId);
            if (medewerker == null)
                throw new KeyNotFoundException("Medewerker niet gevonden.");

            medewerker.FrontofficeMedewerkers.Remove(medewerker);
            frontOfficeMedewerkerRepository.Update(medewerker);
            frontOfficeMedewerkerRepository.Save();

            string bericht = $"Beste {medewerker.medewerkerNaam},\n\nU bent verwijderd uit het bedrijf CarAndAll.";
            _emailService.SendEmail(medewerker.medewerkerEmail, "Medewerker verwijderd", bericht);
        }
    }
}

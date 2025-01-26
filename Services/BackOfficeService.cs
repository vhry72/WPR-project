using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    // Dit is de service voor het beheren van backofficemedewerkers
    public class BackOfficeService
    {
        private readonly IBackOfficeMedewerkerRepository _repository;
        private readonly IEmailService _emailService;
        public BackOfficeService(
            IBackOfficeMedewerkerRepository repository,
            IEmailService emailService)
        {
            // Initialiseer de repository en de e-mailservice
            _repository = repository;
            _emailService = emailService;
        }

        // Haal de gegevens van een backofficemedewerker op via het ID
        public BackofficeMedewerkerWijzigDTO GetGegevensById(Guid id)
        {
            // Zoek de medewerker in de repository
            var huurder = _repository.GetBackofficemedewerkerById(id);
            // Controleer of de medewerker bestaat
            if (huurder == null) return null;

            // Construeer een DTO-object met de benodigde gegevens
            return new BackofficeMedewerkerWijzigDTO
            {
                medewerkerEmail = huurder.medewerkerEmail,
                medewerkerNaam = huurder.medewerkerNaam,

            };

        }
        // Werk de gegevens van een backofficemedewerker bij
        public void Update(Guid id, BackofficeMedewerkerWijzigDTO dto)
        {
            // Haal de medewerker op via de repository
            var huurder = _repository.GetBackofficemedewerkerById(id);
            // Controleer of de medewerker bestaat
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");


            huurder.medewerkerNaam = dto.medewerkerNaam;
            huurder.medewerkerEmail = dto.medewerkerEmail;

            // Update de medewerker in de repository en sla de wijzigingen op
            _repository.UpdateBackOfficeMedewerker(huurder);
            _repository.Save();
        }

        // Deactiveer het account van een backofficemedewerker en stuur een bevestigingsmail
        public void Delete(Guid id)
        {
            // Haal de medewerker op via de repository
            var huurder = _repository.GetBackofficemedewerkerById(id);
            // Controleer of het ID geldig is
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID is verplicht.");
            }

            // Deactiveer het account van de medewerker
            _repository.DeactivateBackOfficeMedewerker(id);

            // Stuur een e-mail naar de medewerker om de deactivatie te bevestigen
            string bericht = $"Beste {huurder.medewerkerNaam},\n\n Uw account wordt verwijderd, \n\n Vriendelijke Groet, \n CarAndAll";
            _emailService.SendEmail(huurder.medewerkerEmail, "Account verwijderd", bericht);
        }

        // Voer een keuring uit op een abonnement
        public void KeuringAbonnement(Guid id, bool keuring)
        {
            try
            {
                // Voer de keuring uit via de repository
                _repository.AbonnementKeuring(id, keuring);

            }
            catch (Exception e)
            {
                // Gooi een foutmelding als er iets misgaat
                throw new InvalidOperationException(e.Message);

            }
        }
    }
}

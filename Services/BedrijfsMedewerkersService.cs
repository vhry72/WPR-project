using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{ // Service voor het beheren van bedrijfsmedewerkers
    public class BedrijfsMedewerkersService
    {
        private readonly IBedrijfsMedewerkersRepository _repository;
        private readonly IEmailService _emailService;
        private readonly ISchademeldingRepository _schaderepository;

        public BedrijfsMedewerkersService(IBedrijfsMedewerkersRepository repository, IEmailService emailService, ISchademeldingRepository schademeldingRepository)
        {
            // Initialiseer de repositories en services
            _repository = repository;
            _emailService = emailService;
            _schaderepository = schademeldingRepository;
        }

        public BedrijfsMedewerkersDTO GetById(Guid id)
        {
            // Haal een specifieke bedrijfsmedewerker op via ID
            var medewerker = _repository.GetMedewerkerById(id);
            // Controleer of de medewerker bestaat
            if (medewerker == null) return null;

            // Maak een DTO-object van de medewerkergegevens
            return new BedrijfsMedewerkersDTO
            {
                bedrijfsMedewerkerId = medewerker.bedrijfsMedewerkerId,
                medewerkerNaam = medewerker.medewerkerNaam,
                medewerkerEmail = medewerker.medewerkerEmail,
                abonnementId = medewerker.AbonnementId,
            };
        }

        // Haal de gegevens van een bedrijfsmedewerker op voor bewerking
        public BedrijfsMedewerkerWijzigDTO GetGegevensById(Guid id)
        {
            // Zoek de medewerker in de repository
            var huurder = _repository.GetMedewerkerById(id);
            // Controleer of de medewerker bestaat
            if (huurder == null) return null;

            // Maak een DTO-object van de medewerkergegevens voor wijziging
            return new BedrijfsMedewerkerWijzigDTO
            {
                medewerkerEmail = huurder.medewerkerEmail,
                medewerkerNaam = huurder.medewerkerNaam,
            };

        }


        // Werk de gegevens van een bedrijfsmedewerker bij
        public void Update(Guid id, BedrijfsMedewerkerWijzigDTO dto)
        {
            // Haal de medewerker op via de repository
            var huurder = _repository.GetMedewerkerById(id);
            // Controleer of de medewerker bestaat
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");

            // Maak een DTO-object van de medewerkergegevens voor wijziging
            huurder.medewerkerNaam = dto.medewerkerNaam;
            huurder.medewerkerEmail = dto.medewerkerEmail;

            // Update de medewerker in de repository en sla de wijzigingen op
            _repository.Update(huurder);
            _repository.Save();
        }

        // Deactiveer het account van een bedrijfsmedewerker en stuur een bevestigingsmail
        public void Delete(Guid id)
        {
            // Controleer of het ID geldig is
            if (id == Guid.Empty)
            {
               
                throw new ArgumentException("ID is verplicht.");
            }

            try
            {
                // Zoek de medewerker in de repository
                var bedrijfsMedewerker = _repository.GetMedewerkerById(id);
                // Zoek de medewerker in de repository
                if (bedrijfsMedewerker == null)
                {
                    throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden.");
                }

                // Deactiveer het account van de medewerker
                _repository.Deactivate(id);

                // Stuur een bevestigingsmail naar de medewerker
                string bericht = $"Beste {bedrijfsMedewerker.medewerkerNaam},\n\nUw account is verwijderd.\n\nVriendelijke Groet,\nCarAndAll";
                _emailService.SendEmail(bedrijfsMedewerker.medewerkerEmail, "Account verwijderd", bericht);
            }
            catch (InvalidOperationException ex)
            {
                // Gooi een foutmelding als er iets misgaat bij de deactivatie
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}

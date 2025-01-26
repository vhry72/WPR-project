using WPR_project.Data;
using WPR_project.DTO_s;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    // Service voor het beheren van frontofficemedewerkers en hun acties
    public class FrontOfficeService
    {
        private readonly IFrontOfficeMedewerkerRepository _frontOfficeMedewerkerRepository;
        private readonly IEmailService _emailService;

        // Constructor: initialiseert de benodigde repositories en services
        public FrontOfficeService(
            IFrontOfficeMedewerkerRepository frontOfficeMedewerkerRepository,
            IEmailService emailService)
        {
            _frontOfficeMedewerkerRepository = frontOfficeMedewerkerRepository;
            _emailService = emailService;
        }
        // Haal alle frontofficemedewerkers op
        public IQueryable GetAll()
        {
            return _frontOfficeMedewerkerRepository.Getall();
        }

        // Verwijder een frontofficemedewerker (deactiveer het account)
        public void Delete(Guid id)
        {
            var medewerker = _frontOfficeMedewerkerRepository.GetFrontOfficeMedewerkerById(id);
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID is verplicht.");
            }
            _frontOfficeMedewerkerRepository.DeactivateFrontOffice(id);
            string bericht = $"Beste {medewerker.medewerkerNaam},\n\n Uw account wordt verwijderd, \n\n Vriendelijke Groet, \n CarAndAll";
            _emailService.SendEmail(medewerker.medewerkerEmail, "Medewerker verwijderd", bericht);
        }

        // Haal de gegevens van een frontofficemedewerker op via ID
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

        // Werk de gegevens van een frontofficemedewerker bij
        public void Update(Guid id, FrontofficeMedewerkerWijzigDTO dto)
        {
            var huurder = _frontOfficeMedewerkerRepository.GetFrontOfficeMedewerkerById(id);
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");

            huurder.medewerkerNaam = dto.medewerkerNaam;
            huurder.medewerkerEmail = dto.medewerkerEmail;

            _frontOfficeMedewerkerRepository.Update(huurder);
            _frontOfficeMedewerkerRepository.Save();
        }

        // Markeer een huurverzoek als voltooid
        public void HuurverzoekIsCompleted(Guid HuurverzoekId, bool keuring)
        {
            try
            {
                _frontOfficeMedewerkerRepository.HuurverzoekIsCompleted(HuurverzoekId, keuring);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        // Markeer een schademelding als voltooid
        public void schademeldingIsCompleted(Guid schademeldingId, bool keuring)
        {
            try
            {
                _frontOfficeMedewerkerRepository.schademeldingIsCompleted(schademeldingId, keuring);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}


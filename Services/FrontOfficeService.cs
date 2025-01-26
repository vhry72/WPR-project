using WPR_project.Data;
using WPR_project.DTO_s;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class FrontOfficeService
    {
        private readonly IFrontOfficeMedewerkerRepository _frontOfficeMedewerkerRepository;
        private readonly IEmailService _emailService;

        public FrontOfficeService(
            IFrontOfficeMedewerkerRepository frontOfficeMedewerkerRepository,
            IEmailService emailService)
        {
            _frontOfficeMedewerkerRepository = frontOfficeMedewerkerRepository;
            _emailService = emailService;
        }

        public IQueryable GetAll()
        {
            return _frontOfficeMedewerkerRepository.Getall();
        }

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


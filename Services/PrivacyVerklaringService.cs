using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class PrivacyVerklaringService
    {
        private readonly IPrivacyVerklaringRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IBackOfficeMedewerkerRepository _backOfficeRepository;

        public PrivacyVerklaringService(
            IPrivacyVerklaringRepository repository, 
            IEmailService emailService, 
            IBackOfficeMedewerkerRepository backOfficeRepository)
        {
            _repository = repository;
            _emailService = emailService;
            _backOfficeRepository = backOfficeRepository;
        }

        public PrivacyVerklaring GetLatestPrivacyVerklaring()
        {
            return _repository.GetLatestPrivacyVerklaring();
        }

        public void Add(PrivacyVerklaringDTO privacyVerklaringdto)
        {
            var privacyVerklaring = new PrivacyVerklaring
            {
                Verklaring = ConvertNewLines(privacyVerklaringdto.Verklaring),
                UpdateDatum = DateTime.Now,
                MedewerkerId = privacyVerklaringdto.MedewerkerId,
                VerklaringId = Guid.NewGuid(),
            };

            SendEmailToBackOffice(privacyVerklaringdto.MedewerkerId);
            _repository.Add(privacyVerklaring);
        }

        public IQueryable<PrivacyVerklaring> GetAllPrivacyVerklaringen()
        {
            return _repository.GetAllPrivacyVerklaringen();
        }

        public string ConvertNewLines(string input)
        {
            return input.Replace("\\n", "\n");
        }


        public void SendEmailToBackOffice(Guid MedewerkerId)
        {
            var backOfficeMedewerker = _backOfficeRepository.GetBackofficemedewerkerById(MedewerkerId);
            var privacyVerklaring = GetLatestPrivacyVerklaring();
            var privacyverklaringText = privacyVerklaring.Verklaring;
            var subject = "Nieuwe Privacy Verklaring";
            var body = $"Beste {backOfficeMedewerker.medewerkerNaam},\n\nEr is een nieuwe privacy verklaring toegevoegd. Zie Hieronder het nieuwe privacyverklaring\n {privacyverklaringText}\n\nMet vriendelijke groet,\n\n CarAndAll";
            _emailService.SendEmail(backOfficeMedewerker.medewerkerEmail, subject, body);
        }

    }
}

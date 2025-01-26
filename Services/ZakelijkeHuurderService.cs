using System.ComponentModel.DataAnnotations;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class ZakelijkeHuurderService
    {
        private readonly IZakelijkeHuurderRepository _repository;
        private readonly IWagenparkBeheerderRepository _wagenparkBeheerderRepository;
        private readonly IEmailService _emailService;

        public ZakelijkeHuurderService(IZakelijkeHuurderRepository repository, IWagenparkBeheerderRepository wagenparkBeheerderRepository, IEmailService emailService)
        {
            _repository = repository;
            _wagenparkBeheerderRepository = wagenparkBeheerderRepository;
            _emailService = emailService;
        }

        
        public ZakelijkHuurder GetZakelijkHuurderById(Guid id)
        {
            return _repository.GetZakelijkHuurderById(id);
        }

        public List<WagenparkBeheerderGetGegevensDTO> GetWagenparkBeheerdersByZakelijkeId(Guid id)
        {
            var wagenparkbeheerders = _repository.GetWagenparkBeheerdersByZakelijkeId(id);

            if (wagenparkbeheerders == null || !wagenparkbeheerders.Any())
            {
                throw new InvalidOperationException("Deze zakelijkeID heeft geen wagenparkbeheerders");
            }

            var resultaat = new List<WagenparkBeheerderGetGegevensDTO>();

            foreach (var wb in wagenparkbeheerders)
            {
                resultaat.Add(new WagenparkBeheerderGetGegevensDTO
                {
                    beheerderId = wb.beheerderId,
                    beheerderNaam = wb.beheerderNaam,
                    bedrijfsEmail = wb.bedrijfsEmail,
                    IsActive = wb.IsActive
                });
            }

            return resultaat;
        }


        public void RegisterZakelijkeHuurder(ZakelijkHuurder huurder)
        {
            var validationContext = new ValidationContext(huurder);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(huurder, validationContext, validationResults, true))
            {
                var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                throw new ArgumentException($"Validatie mislukt: {string.Join(", ", errors)}");
            }

            _repository.AddZakelijkHuurder(huurder);
            _repository.Save();

            var verificatieUrl = $"https://localhost:5033/api/ZakelijkeHuurder/verify?token={huurder.EmailBevestigingToken}";
            var emailBody = $"Beste {huurder.bedrijfsNaam},<br><br>Klik op de volgende link om je e-mailadres te bevestigen:<br><a href='{verificatieUrl}'>Bevestig e-mail</a>";
            _emailService.SendEmail(huurder.bedrijfsEmail, "Bevestig je registratie", emailBody);
        }

        public Guid? GetAbonnementIdByZakelijkeHuurder(Guid id)
        {
            return _repository.GetAbonnementIdByZakelijkeHuurder(id);
            
        }
    

        public void Update(Guid id, ZakelijkeHuurderWijzigDTO dto)
        {
            var huurder = _repository.GetZakelijkHuurderById(id);
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");


            huurder.bedrijfsNaam = dto.bedrijfsNaam;
            huurder.bedrijfsEmail = dto.bedrijfsEmail;
            huurder.adres = dto.adres;
            huurder.telNummer = dto.telNummer;
            huurder.KVKNummer = dto.KVKNummer;



            _repository.UpdateZakelijkHuurder(huurder);
            _repository.Save();
        }

        public ZakelijkeHuurderWijzigDTO GetGegevensById(Guid id)
        {
            var huurder = _repository.GetZakelijkHuurderById(id);
            if (huurder == null) return null;

            return new ZakelijkeHuurderWijzigDTO
            {
                bedrijfsEmail = huurder.bedrijfsEmail,
                bedrijfsNaam = huurder.bedrijfsNaam,
                KVKNummer = huurder.KVKNummer,
                adres = huurder.adres,
                telNummer = huurder.telNummer,

            };

        }


        // Verwijder een zakelijke huurder
        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID is verplicht.");
            }
            _repository.ScheduleDeleteZakelijkHuurder(id);
        }
    }
}
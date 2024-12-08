using Microsoft.EntityFrameworkCore;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class ParticulierHuurderService
    {
        private readonly IHuurderRegistratieRepository _repository;
        private readonly IEmailService _emailService;

        public ParticulierHuurderService(IHuurderRegistratieRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public IEnumerable<ParticulierHuurderDTO> GetAll()
        {
            return _repository.GetAll().Select(h => new ParticulierHuurderDTO
            {
                particulierId = h.particulierId,
                particulierNaam = h.particulierNaam,
                particulierEmail = h.particulierEmail,
                IsEmailBevestigd = h.IsEmailBevestigd
            });
        }

        public ParticulierHuurderDTO GetById(Guid id)
        {
            var huurder = _repository.GetById(id);
            if (huurder == null) return null;

            return new ParticulierHuurderDTO
            {
                particulierId = huurder.particulierId,
                particulierNaam = huurder.particulierNaam,
                particulierEmail = huurder.particulierEmail,
                IsEmailBevestigd = huurder.IsEmailBevestigd
            };
        }

        public void Register(ParticulierHuurder particulierH)
        {
            if (string.IsNullOrWhiteSpace(particulierH.particulierEmail) || string.IsNullOrWhiteSpace(particulierH.wachtwoord))
            {
                throw new ArgumentException("E-mailadres en wachtwoord zijn verplicht.");
            }

            var huurder = new ParticulierHuurder
            {
                particulierId = Guid.NewGuid(),
                particulierEmail = particulierH.particulierEmail,
                particulierNaam = particulierH.particulierNaam,
                wachtwoord = particulierH.wachtwoord,
                adress = particulierH.adress,
                postcode = particulierH.postcode,
                woonplaats = particulierH.woonplaats,
                telefoonnummer = particulierH.telefoonnummer,
                EmailBevestigingToken = Guid.NewGuid().ToString(),
                IsEmailBevestigd = false,
            };

            _repository.Add(huurder);
            _repository.Save();

            // Verificatiemail
            var verificatieUrl = $"https://localhost:5033/api/ParticulierHuurder/verify?token={huurder.EmailBevestigingToken}";
            var emailBody = $"Beste {huurder.particulierNaam},<br><br>Klik op de volgende link om je e-mailadres te bevestigen:<br><a href='{verificatieUrl}'>Bevestig je e-mail</a>";
            _emailService.SendEmail(huurder.particulierEmail, "Bevestig je registratie", emailBody);
        }

        public bool VerifyEmail(string token)
        {
            var huurder = _repository.GetByToken(token);
            if (huurder == null || huurder.IsEmailBevestigd) return false;

            huurder.IsEmailBevestigd = true;
            _repository.Update(huurder);
            _repository.Save();
            return true;
        }

        public void Update(Guid id, ParticulierHuurderDTO dto)
        {
            var huurder = _repository.GetById(id);
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");

            huurder.particulierNaam = dto.particulierNaam;
            huurder.particulierEmail = dto.particulierEmail;

            _repository.Update(huurder);
            _repository.Save();
        }

        public void Delete(Guid id)
        {
            var huurder = _repository.GetById(id);
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");

            _repository.Delete(huurder);
            _repository.Save();
        }
    }
}

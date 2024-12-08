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
                particulierEmail = h.particulierEmail
            });
        }

        public ParticulierHuurderDTO GetById(int id)
        {
            var huurder = _repository.GetById(id);
            if (huurder == null) return null;

            return new ParticulierHuurderDTO
            {
                particulierId = huurder.particulierId,
                particulierNaam = huurder.particulierNaam,
                particulierEmail = huurder.particulierEmail
            };
        }

        public void Register(ParticulierHuurder pdto)
        {
            var huurder = new ParticulierHuurder
            {
                particulierEmail = pdto.particulierEmail,
                particulierNaam = pdto.particulierNaam,
                wachtwoord = pdto.wachtwoord,
                adress = pdto.adress,
                postcode = pdto.postcode,
                woonplaats = pdto.woonplaats,
                telefoonnummer = pdto.telefoonnummer,
                EmailBevestigingToken = Guid.NewGuid().ToString(), // Genereer verificatietoken
                IsEmailBevestigd = false,
            };

            Console.WriteLine(huurder);


            _repository.Add(huurder);
            _repository.Save();

            // Stuur een verificatiemail
            var verificatieUrl = $"https://jouwdomein.com/api/ParticulierHuurder/verify?token={huurder.EmailBevestigingToken}";
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

        public void Update(int id, ParticulierHuurderDTO dto)
        {
            var huurder = _repository.GetById(id);
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");

            huurder.particulierNaam = dto.particulierNaam;
            huurder.particulierEmail = dto.particulierEmail;

            _repository.Update(huurder);
            _repository.Save();
        }

        public void Delete(int id)
        {
            var huurder = _repository.GetById(id);
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");

            _repository.Delete(huurder);
            _repository.Save();
        }
    }
}

using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class ZakelijkeHuurderService
    {
        private readonly IZakelijkeHuurderRepository _repository;
        private readonly IEmailService _emailService;

        public ZakelijkeHuurderService(IZakelijkeHuurderRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public IEnumerable<ZakelijkHuurder> GetAllZakelijkHuurders()
        {
            return _repository.GetAllZakelijkHuurders().Select(h => new ZakelijkHuurder
            {
                bedrijfsNaam = h.bedrijfsNaam,
                adres = h.adres,
                KVKNummer = h.KVKNummer
            });
        }

        public ZakelijkHuurder GetZakelijkHuurderById(int id)
        {
            var huurder = _repository.GetZakelijkHuurderById(id);
            if (huurder == null)
            {
                return null;
            }
            return new ZakelijkHuurder
            {
                bedrijfsNaam = huurder.bedrijfsNaam,
                adres = huurder.adres,
                KVKNummer = huurder.KVKNummer
            };
        }

        public void AddZakelijkHuurder(ZakelijkHuurder zkh)
        {
            var zakelijkeHuurder = new ZakelijkHuurder
            {
                bedrijfsNaam = zkh.bedrijfsNaam,
                adres = zkh.adres,
                KVKNummer = zkh.KVKNummer
            };
            _repository.AddZakelijkHuurder(zakelijkeHuurder);
            _repository.Save();

            // Stuur een e-mailbevestiging
            var emailBody = $"Beste {zkh.bedrijfsNaam},\n\nUw bedrijfsaccount is succesvol aangemaakt.\n\nBedankt!";
            _emailService.SendEmail(zkh.KVKNummer + "@example.com", "Bevestiging Bedrijfsaccount", emailBody);
        }

        public void Update(int id, ZakelijkHuurder zkh)
        {
            var zakelijkeHuurder = _repository.GetZakelijkHuurderById(id);
            if (zakelijkeHuurder != null)
            {
                zakelijkeHuurder.bedrijfsNaam = zkh.bedrijfsNaam;
                zakelijkeHuurder.adres = zkh.adres;
                zakelijkeHuurder.KVKNummer = zkh.KVKNummer;
                _repository.UpdateZakelijkHuurder(zakelijkeHuurder);
                _repository.Save();
            }
        }

        public void Delete(int id)
        {
            _repository.DeleteZakelijkHuurder(id);
            _repository.Save();
        }

        public void VoegMedewerkerToe(int zakelijkeHuurderId, string medewerkerEmail)
        {
            var zakelijkeHuurder = _repository.GetZakelijkHuurderById(zakelijkeHuurderId);
            if (zakelijkeHuurder != null)
            {
                // Controleer of het e-mailadres al bestaat
                if (!zakelijkeHuurder.MedewerkersEmails.Contains(medewerkerEmail))
                {
                    zakelijkeHuurder.MedewerkersEmails.Add(medewerkerEmail);
                    _repository.UpdateZakelijkHuurder(zakelijkeHuurder);
                    _repository.Save();

                    // Stuur notificatie naar de medewerker
                    var emailBody = $"Beste medewerker,\n\nU bent toegevoegd aan het bedrijfsaccount van {zakelijkeHuurder.bedrijfsNaam}.";
                    _emailService.SendEmail(medewerkerEmail, "Medewerker Toegevoegd", emailBody);
                }
                else
                {
                    throw new InvalidOperationException("Medewerker bestaat al in de lijst.");
                }
            }
            else
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");
            }
        }

        public void VerwijderMedewerker(int zakelijkeHuurderId, string medewerkerEmail)
        {
            var zakelijkeHuurder = _repository.GetZakelijkHuurderById(zakelijkeHuurderId);
            if (zakelijkeHuurder != null)
            {
                // Controleer of het e-mailadres bestaat
                if (zakelijkeHuurder.MedewerkersEmails.Contains(medewerkerEmail))
                {
                    zakelijkeHuurder.MedewerkersEmails.Remove(medewerkerEmail);
                    _repository.UpdateZakelijkHuurder(zakelijkeHuurder);
                    _repository.Save();

                    // Stuur notificatie naar de medewerker
                    var emailBody = $"Beste medewerker,\n\nU bent verwijderd uit het bedrijfsaccount van {zakelijkeHuurder.bedrijfsNaam}.";
                    _emailService.SendEmail(medewerkerEmail, "Medewerker Verwijderd", emailBody);
                }
                else
                {
                    throw new InvalidOperationException("Medewerker niet gevonden in de lijst.");
                }
            }
            else
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden in het systeem.");
            }
        }
    }
}

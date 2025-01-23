using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        // Haal alle zakelijke huurders op
        public IEnumerable<ZakelijkHuurder> GetAllZakelijkHuurders()
        {
            return _repository.GetAllZakelijkHuurders();
        }

        // Haal een specifieke zakelijke huurder op via ID
        public ZakelijkHuurder GetZakelijkHuurderById(Guid id)
        {
            return _repository.GetZakelijkHuurderById(id);
        }

        // Registreer een nieuwe zakelijke huurder met e-mailverificatie
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

        public bool VerifyEmail(Guid token)
        {
            var huurder = _repository.GetZakelijkHuurderByToken(token);
            if (huurder == null || huurder.IsEmailBevestigd)
            {
                return false; // Token ongeldig 
            }

            // Update verificatiestatus
            huurder.IsEmailBevestigd = true;
            _repository.UpdateZakelijkHuurder(huurder);
            _repository.Save();
            return true;
        }

        public Guid? GetAbonnementIdByZakelijkeHuurder(Guid id)
        {
            return _repository.GetAbonnementIdByZakelijkeHuurder(id);
            
        }
    

        // Voeg een zakelijke huurder toe (zonder e-mailverificatie)
        public void AddZakelijkeHuurder(ZakelijkHuurder huurder)
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
        }

        // Voeg een wagenparkbeheerder toe aan een zakelijke huurder
        public void AddWagenparkBeheerder(Guid zakelijkeHuurderId, WagenparkBeheerder beheerder)
        {
            var zakelijkeHuurder = _repository.GetZakelijkHuurderById(zakelijkeHuurderId);
            if (zakelijkeHuurder == null)
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");
            }

            if (!beheerder.bedrijfsEmail.EndsWith($"@{zakelijkeHuurder.bedrijfsEmail.Split('@')[1]}"))
            {
                throw new ArgumentException("Het e-mailadres van de wagenparkbeheerder moet overeenkomen met het domein van de zakelijke huurder.");
            }

            _wagenparkBeheerderRepository.AddWagenparkBeheerder(beheerder);
            _wagenparkBeheerderRepository.Save();
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
            _repository.DeleteZakelijkHuurder(id);
        }

        // Voeg een medewerker toe aan een zakelijke huurder
        public void VoegMedewerkerToe(Guid zakelijkeId, string medewerkerNaam, string medewerkerEmail)
        {
            // Controleer of het e-mailadres geldig is
            if (string.IsNullOrWhiteSpace(medewerkerEmail) || !medewerkerEmail.Contains("@"))
            {
                throw new ArgumentException("Ongeldig e-mailadres.");
            }

            // Haal de zakelijke huurder op
            var huurder = _repository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");
            }

            // Controleer of de medewerker al bestaat op basis van het e-mailadres
            if (huurder.Medewerkers.Any(m => m.medewerkerEmail == medewerkerEmail))
            {
                throw new InvalidOperationException("Deze medewerker bestaat al.");
            }

            // Maak een nieuwe medewerker aan
            var nieuweMedewerker = new BedrijfsMedewerkers
            {
                bedrijfsMedewerkerId = Guid.NewGuid(),
                medewerkerNaam = medewerkerNaam,
                medewerkerEmail = medewerkerEmail,
                zakelijkeId = huurder.zakelijkeId
            };

            // Voeg de medewerker toe aan de lijst van medewerkers
            huurder.Medewerkers.Add(nieuweMedewerker);

            // Update de huurder in de repository
            _repository.UpdateZakelijkHuurder(huurder);
            _repository.Save();

            // Stuur een notificatie naar de medewerker
            var emailBody = $"Beste {medewerkerNaam},\n\nU bent toegevoegd aan het bedrijfsaccount van {huurder.bedrijfsNaam}.\nU kunt nu gebruik maken van de voordelen van het bedrijfsabonnement.\n\nMet vriendelijke groet,\nHet Bedrijfsteam";
            _emailService.SendEmail(medewerkerEmail, "Medewerker Toegevoegd", emailBody);
        }

        // Verwijder een medewerker van een zakelijke huurder
        public void VerwijderMedewerker(Guid zakelijkeId, string medewerkerEmail)
        {
            // Haal de zakelijke huurder op
            var huurder = _repository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");
            }

            // Controleer of de medewerker bestaat op basis van e-mail
            var medewerker = huurder.Medewerkers.FirstOrDefault(m => m.medewerkerEmail == medewerkerEmail);
            if (medewerker == null)
            {
                throw new InvalidOperationException("Medewerker niet gevonden.");
            }

            // Verwijder de medewerker uit de lijst
            huurder.Medewerkers.Remove(medewerker);

            // Update de huurder in de repository
            _repository.UpdateZakelijkHuurder(huurder);
            _repository.Save();

            // Stuur een notificatie naar de medewerker
            var emailBody = $"Beste {medewerker.medewerkerNaam},\n\nU bent verwijderd uit het bedrijfsaccount van {huurder.bedrijfsNaam}.\nU kunt niet langer gebruik maken van de voordelen van het bedrijfsabonnement.\n\nMet vriendelijke groet,\nHet Bedrijfsteam";
            _emailService.SendEmail(medewerkerEmail, "Medewerker Verwijderd", emailBody);
        }
    }
}
﻿using System.ComponentModel.DataAnnotations;
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

            if(!Validator.TryValidateObject(huurder, validationContext, validationResults, true))
            {
                var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                throw new ArgumentException($"Validatie mislukt: {string.Join(", ", errors)}");
            }

            huurder.EmailBevestigingToken = Guid.NewGuid().ToString();
            huurder.IsEmailBevestigd = false;

            
            _repository.AddZakelijkHuurder(huurder);
            _repository.Save();

            
            var verificatieUrl = $"https://localhost:5033/api/ZakelijkeHuurder/verify?token={huurder.EmailBevestigingToken}";
            var emailBody = $"Beste {huurder.bedrijfsNaam},<br><br>Klik op de volgende link om je e-mailadres te bevestigen:<br><a href='{verificatieUrl}'>Bevestig e-mail</a>";
            _emailService.SendEmail(huurder.email, "Bevestig je registratie", emailBody);
        }

        
        public bool VerifyEmail(string token)
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

        // Voeg een zakelijke huurder toe (zonder e-mailverificatie)
        public void AddZakelijkHuurder(ZakelijkHuurder huurder)
        {
            _repository.AddZakelijkHuurder(huurder);
            _repository.Save();
        }

        // Werk een zakelijke huurder bij
        public void Update(Guid id, ZakelijkHuurder updatedHuurder)
        {
            var bestaandeHuurder = _repository.GetZakelijkHuurderById(id);
            if (bestaandeHuurder == null)
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");
            }

            bestaandeHuurder.bedrijfsNaam = updatedHuurder.bedrijfsNaam;
            bestaandeHuurder.adres = updatedHuurder.adres;
            bestaandeHuurder.KVKNummer = updatedHuurder.KVKNummer;
            bestaandeHuurder.telNummer = updatedHuurder.telNummer;
            bestaandeHuurder.email = updatedHuurder.email;

            _repository.UpdateZakelijkHuurder(bestaandeHuurder);
            _repository.Save();
        }

        // Verwijder een zakelijke huurder
        public void Delete(Guid id)
        {
            var huurder = _repository.GetZakelijkHuurderById(id);
            if (huurder == null)
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");
            }

            _repository.DeleteZakelijkHuurder(id);
            _repository.Save();
        }

        // Voeg een medewerker toe aan een zakelijke huurder
        public void VoegMedewerkerToe(Guid zakelijkeId, string medewerkerEmail)
        {
            var huurder = _repository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");
            }

            if (huurder.MedewerkersEmails.Contains(medewerkerEmail))
            {
                throw new InvalidOperationException("Medewerker bestaat al.");
            }

            huurder.MedewerkersEmails.Add(medewerkerEmail);
            _repository.UpdateZakelijkHuurder(huurder);
            _repository.Save();

            // Stuur notificatie naar de medewerker
            var emailBody = $"Beste medewerker,<br><br>U bent toegevoegd aan het bedrijfsaccount van {huurder.bedrijfsNaam}.";
            _emailService.SendEmail(medewerkerEmail, "Medewerker Toegevoegd", emailBody);
        }

        // Verwijder een medewerker van een zakelijke huurder
        public void VerwijderMedewerker(Guid zakelijkeId, string medewerkerEmail)
        {
            var huurder = _repository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");
            }

            if (!huurder.MedewerkersEmails.Contains(medewerkerEmail))
            {
                throw new InvalidOperationException("Medewerker niet gevonden.");
            }

            huurder.MedewerkersEmails.Remove(medewerkerEmail);
            _repository.UpdateZakelijkHuurder(huurder);
            _repository.Save();

            // Stuur notificatie naar de medewerker
            var emailBody = $"Beste medewerker,<br><br>U bent verwijderd uit het bedrijfsaccount van {huurder.bedrijfsNaam}.";
            _emailService.SendEmail(medewerkerEmail, "Medewerker Verwijderd", emailBody);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using WPR_project.Data;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class WagenparkBeheerderService
    {
        private readonly IWagenparkBeheerderRepository _repository;
        private readonly IZakelijkeHuurderRepository _zakelijkeHuurderRepository;
        private readonly IAbonnementRepository _abonnementRepository;
        private readonly IEmailService _emailService;
        private readonly GegevensContext _context;

        public WagenparkBeheerderService(
            IWagenparkBeheerderRepository repository,
            IZakelijkeHuurderRepository zakelijkeHuurderRepository,
            IAbonnementRepository abonnementRepository,
            GegevensContext context,
            IEmailService emailService)
        {
            _repository = repository;
            _zakelijkeHuurderRepository = zakelijkeHuurderRepository;
            _abonnementRepository = abonnementRepository;
            _context = context;
            _emailService = emailService;
        }

        public IEnumerable<WagenparkBeheerder> GetWagenparkBeheerders()
        {
            return _repository.GetWagenparkBeheerders();
        }

        public WagenparkBeheerder GetBeheerderById(Guid id)
        {
            return _repository.getBeheerderById(id);
        }

        public void AddWagenparkBeheerder(WagenparkBeheerder beheerder)
        {
            _repository.AddWagenparkBeheerder(beheerder);
            _repository.Save();
        }

        public void UpdateWagenparkBeheerder(Guid id, WagenparkBeheerder beheerder)
        {
            var existingBeheerder = _repository.getBeheerderById(id);
            if (existingBeheerder != null)
            {
                existingBeheerder.beheerderNaam = beheerder.beheerderNaam;
                existingBeheerder.bedrijfsEmail = beheerder.bedrijfsEmail;
                existingBeheerder.telefoonNummer = beheerder.telefoonNummer;

                _repository.UpdateWagenparkBeheerder(existingBeheerder);
                _repository.Save();
            }
            else
            {
                throw new KeyNotFoundException("Beheerder niet gevonden.");
            }
        }

        public void DeleteWagenparkBeheerder(Guid id)
        {
            var beheerder = _repository.getBeheerderById(id);
            if (beheerder != null)
            {
                _repository.DeleteWagenparkBeheerder(id);
                _repository.Save();
            }
            else
            {
                throw new KeyNotFoundException("Beheerder niet gevonden.");
            }
        }

        // Voeg een medewerker toe aan een zakelijke huurder
        public void VoegMedewerkerToe(Guid zakelijkeId, string medewerkerNaam, string medewerkerEmail)
        {
            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

            if (huurder.Medewerkers.Any(m => m.medewerkerEmail == medewerkerEmail))
                throw new InvalidOperationException("Deze medewerker bestaat al.");

            var medewerker = new BedrijfsMedewerkers
            {
                BedrijfsMedewerkId = Guid.NewGuid(),
                medewerkerNaam = medewerkerNaam,
                medewerkerEmail = medewerkerEmail,
                ZakelijkeHuurderId = zakelijkeId
            };

            huurder.Medewerkers.Add(medewerker);
            _zakelijkeHuurderRepository.UpdateZakelijkHuurder(huurder);
            _zakelijkeHuurderRepository.Save();

            string bericht = $"Beste {medewerkerNaam},\n\nU bent toegevoegd aan het bedrijfsaccount van {huurder.bedrijfsNaam}.";
            _emailService.SendEmail(medewerkerEmail, "Welkom bij het bedrijfsaccount", bericht);
        }

        // Verwijder een medewerker van een zakelijke huurder
        public void VerwijderMedewerker(Guid zakelijkeId, Guid medewerkerId)
        {
            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

            var medewerker = huurder.Medewerkers.FirstOrDefault(m => m.BedrijfsMedewerkId == medewerkerId);
            if (medewerker == null)
                throw new KeyNotFoundException("Medewerker niet gevonden.");

            huurder.Medewerkers.Remove(medewerker);
            _zakelijkeHuurderRepository.UpdateZakelijkHuurder(huurder);
            _zakelijkeHuurderRepository.Save();

            string bericht = $"Beste {medewerker.medewerkerNaam},\n\nU bent verwijderd uit het bedrijfsaccount van {huurder.bedrijfsNaam}.";
            _emailService.SendEmail(medewerker.medewerkerEmail, "Medewerker verwijderd", bericht);
        }

        public string GetBeheerderEmailById(Guid zakelijkeId)
        {
            var beheerder = _context.WagenparkBeheerders.FirstOrDefault(b => b.beheerderId == zakelijkeId);
            return beheerder?.bedrijfsEmail;
        }

        public BedrijfsMedewerkers? GetMedewerkerById(Guid medewerkerId)
        {
            return _context.BedrijfsMedewerkers.FirstOrDefault(m => m.BedrijfsMedewerkId == medewerkerId);
        }


        // Voeg een medewerker toe aan een abonnement
        public void VoegMedewerkerAanAbonnementToe(Guid zakelijkeId, Guid medewerkerId, Guid abonnementId)
        {
            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

            var medewerker = huurder.Medewerkers.FirstOrDefault(m => m.BedrijfsMedewerkId == medewerkerId);
            if (medewerker == null)
                throw new KeyNotFoundException("Medewerker niet gevonden.");

            var abonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (abonnement == null)
                throw new KeyNotFoundException("Abonnement niet gevonden.");

            if (abonnement.Medewerkers == null)
                abonnement.Medewerkers = new List<BedrijfsMedewerkers>();

            abonnement.Medewerkers.Add(medewerker);
            _abonnementRepository.UpdateAbonnement(abonnement);
            _abonnementRepository.Save();

            string bericht = $"Beste {medewerker.medewerkerNaam},\n\nU bent toegevoegd aan het abonnement {abonnement.Naam}.";
            _emailService.SendEmail(medewerker.medewerkerEmail, "Toegevoegd aan abonnement", bericht);
        }

        // Haal alle medewerkers van een zakelijke huurder op
        public IEnumerable<BedrijfsMedewerkers> GetMedewerkers(Guid zakelijkeId)
        {
            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

            return huurder.Medewerkers;
        }

        // Haal alle abonnementen van een zakelijke huurder op
        public IEnumerable<Abonnement> GetAbonnementen(Guid beheerderId)
        {
            var huurder = _repository.getBeheerderById(beheerderId);
            if (huurder == null)
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

            return _abonnementRepository.GetAllAbonnementen().Where(a => a.WagenparkBeheerders.Contains(huurder));
        }
    }
}

using System;
using System.Collections.Generic;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    
    public class AbonnementService
    {
        private readonly IAbonnementRepository _abonnementRepository;
        private readonly IZakelijkeHuurderRepository _zakelijkeHuurderRepository;
        private readonly IEmailService _emailService;

        public AbonnementService(
            IAbonnementRepository abonnementRepository,
            IZakelijkeHuurderRepository zakelijkeHuurderRepository,
            IEmailService emailService)
        {
            _abonnementRepository = abonnementRepository;
            _zakelijkeHuurderRepository = zakelijkeHuurderRepository;
            _emailService = emailService;
        }

        /// <summary>
        /// Haalt alle beschikbare abonnementen op.
        /// </summary>
        public IEnumerable<Abonnement> GetAllAbonnementen()
        {
            return _abonnementRepository.GetAllAbonnementen();
        }
        public void VoegMedewerkerToe(Guid zakelijkeId, string medewerkerNaam, string medewerkerEmail)
        {
            // Controleer of het e-mailadres geldig is
            if (string.IsNullOrWhiteSpace(medewerkerEmail) || !medewerkerEmail.Contains("@"))
                throw new ArgumentException("Ongeldig e-mailadres.");

            // Haal de zakelijke huurder op
            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

            // Controleer of het e-maildomein overeenkomt
            var bedrijfDomein = huurder.domein;
            var medewerkerDomein = medewerkerEmail.Split('@')[1];

            if (!bedrijfDomein.Equals(medewerkerDomein, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Medewerker e-mailadres moet het domein {bedrijfDomein} bevatten.");

            // Controleer of de medewerker al bestaat
            if (huurder.Medewerkers.Exists(m => m.medewerkerEmail == medewerkerEmail))
                throw new InvalidOperationException("Deze medewerker is al toegevoegd.");

            // Maak een nieuwe medewerker aan
            var medewerker = new BedrijfsMedewerkers
            {
                BedrijfsMedewerkId = Guid.NewGuid(),
                medewerkerNaam = medewerkerNaam,
                medewerkerEmail = medewerkerEmail,
                ZakelijkeHuurderId = huurder.zakelijkeId
            };

            // Voeg de medewerker toe
            huurder.Medewerkers.Add(medewerker);

            // Update de huurder in de repository
            _zakelijkeHuurderRepository.UpdateZakelijkHuurder(huurder);
            _zakelijkeHuurderRepository.Save();

            // Stuur een e-mail naar de nieuwe medewerker
            string bericht = $@"
    Beste {medewerkerNaam},

    U bent toegevoegd als medewerker bij het bedrijfsabonnement van {huurder.bedrijfsNaam}.
    U kunt nu gebruik maken van de voordelen van dit abonnement.";

            _emailService.SendEmail(medewerkerEmail, "Welkom bij het bedrijfsabonnement", bericht);
        }


        /// <summary>
        /// Verwerkt een betaling voor een Pay-as-you-go abonnement.
        /// </summary>
        public void VerwerkPayAsYouGoBetaling(Guid zakelijkeId, decimal bedrag)
        {
            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

            if (huurder.AbonnementType != AbonnementType.PayAsYouGo)
                throw new InvalidOperationException("Eenmalige betaling is alleen toegestaan voor Pay-As-You-Go abonnementen.");

            // Hier voeg je de logica toe voor eenmalige betaling, zoals het registreren van de transactie.
            string bericht = $@"
    Beste {huurder.bedrijfsNaam},

    Uw eenmalige betaling van €{bedrag:F2} is succesvol verwerkt.

    Datum: {DateTime.Now:dd-MM-yyyy}

    Met vriendelijke groet,
    Het Team";

            _emailService.SendEmail(huurder.bedrijsEmail, "Betaling bevestigd", bericht);
        }


        /// <summary>
        /// Verwerkt een betaling voor een prepaid abonnement.
        /// </summary>
        public void VerwerkPrepaidBetaling(ZakelijkHuurder huurder, decimal kosten)
        {
            if (huurder == null) throw new ArgumentNullException(nameof(huurder));

            if (huurder.PrepaidSaldo < kosten)
                throw new InvalidOperationException("Onvoldoende saldo om deze transactie te voltooien.");

            huurder.PrepaidSaldo -= kosten;
            _zakelijkeHuurderRepository.UpdateZakelijkHuurder(huurder);
            _zakelijkeHuurderRepository.Save();

            string bericht = $@"
            Beste {huurder.bedrijfsNaam},

            Uw prepaid saldo is aangepast.
            Bedrag afgetrokken: €{kosten:F2}
            Nieuw saldo: €{huurder.PrepaidSaldo:F2}

            Datum: {DateTime.Now:dd-MM-yyyy}

            Met vriendelijke groet,
            CarAndAll";

            _emailService.SendEmail(huurder.bedrijsEmail, "Saldo update voor uw prepaid-account", bericht);
        }

        /// <summary>
        /// Laadt prepaid saldo op voor een zakelijke huurder.
        /// </summary>
        public void LaadPrepaidSaldoOp(Guid zakelijkeId, decimal bedrag)
        {
            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

            huurder.PrepaidSaldo += bedrag;
            _zakelijkeHuurderRepository.UpdateZakelijkHuurder(huurder);
            _zakelijkeHuurderRepository.Save();

            string bericht = $@"
            Beste {huurder.bedrijfsNaam},

            Uw prepaid saldo is succesvol opgewaardeerd.
            Bedrag toegevoegd: €{bedrag:F2}
            Nieuw saldo: €{huurder.PrepaidSaldo:F2}

            Datum: {DateTime.Now:dd-MM-yyyy}

            Met vriendelijke groet,
            CarAndAll";

            _emailService.SendEmail(huurder.bedrijsEmail, "Prepaid saldo opgewaardeerd", bericht);
        }

        /// <summary>
        /// Wijzigt het abonnement van een zakelijke huurder.
        /// </summary>
        public void WijzigAbonnement(Guid zakelijkeId, Guid abonnementId)
        {
            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeId);
            if (huurder == null)
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

            var nieuwAbonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (nieuwAbonnement == null)
                throw new KeyNotFoundException("Abonnement niet gevonden.");

            if (huurder.HuidigAbonnement != null)
            {
                var huidigAbonnement = huurder.HuidigAbonnement;
                huidigAbonnement.ZakelijkeHuurders.Remove(huurder);
                _abonnementRepository.UpdateAbonnement(huidigAbonnement);
            }

            huurder.HuidigAbonnement = nieuwAbonnement;
            huurder.AbonnementId = nieuwAbonnement.AbonnementId;
            huurder.updateDatumAbonnement = DateTime.Now;
            nieuwAbonnement.ZakelijkeHuurders.Add(huurder);

            _abonnementRepository.UpdateAbonnement(nieuwAbonnement);
            _zakelijkeHuurderRepository.UpdateZakelijkHuurder(huurder);
            _zakelijkeHuurderRepository.Save();

            StuurBevestigingsEmail(huurder, nieuwAbonnement);
        }

        /// <summary>
        /// Bereken de startdatum van de volgende abonnementsperiode.
        /// </summary>
        public DateTime BerekenVolgendePeriode()
        {
            var huidigeDatum = DateTime.Now;
            return new DateTime(huidigeDatum.Year, huidigeDatum.Month, 1).AddMonths(1);
        }

        private void StuurBevestigingsEmail(ZakelijkHuurder huurder, Abonnement nieuwAbonnement)
        {
            if (huurder == null || nieuwAbonnement == null) throw new ArgumentNullException();

            string subject = "Bevestiging van uw abonnementswijziging";
            string body = $@"
            Beste {huurder.bedrijfsNaam},

            Uw abonnementswijziging is geregistreerd.

            Nieuw abonnement: {nieuwAbonnement.Naam}
            Kosten: €{nieuwAbonnement.Kosten:F2} per maand
            Ingangsdatum: {huurder.updateDatumAbonnement:dd-MM-yyyy}

            Met vriendelijke groet,
            CarAndAll";

            _emailService.SendEmail(huurder.bedrijsEmail, subject, body);
        }
    }
}
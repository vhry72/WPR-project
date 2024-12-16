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
        private readonly IWagenparkBeheerderRepository _wagenparkBeheerderRepository;
        private readonly IEmailService _emailService;

        public AbonnementService(
            IAbonnementRepository abonnementRepository,
            IZakelijkeHuurderRepository zakelijkeHuurderRepository,
            IWagenparkBeheerderRepository wagenparkBeheerderRepository,
            IEmailService emailService)
        {
            _abonnementRepository = abonnementRepository;
            _zakelijkeHuurderRepository = zakelijkeHuurderRepository;
            _wagenparkBeheerderRepository = wagenparkBeheerderRepository;
            _emailService = emailService;
        }

        /// <summary>
        /// Haalt alle beschikbare abonnementen op.
        /// </summary>
        public IEnumerable<Abonnement> GetAllAbonnementen()
        {
            return _abonnementRepository.GetAllAbonnementen();
        }
        public void VoegMedewerkerToe(Guid bedrijfsId, string medewerkerNaam, string medewerkerEmail)
        {
            // Controleer of het e-mailadres geldig is
            if (string.IsNullOrWhiteSpace(medewerkerEmail) || !medewerkerEmail.Contains("@"))
                throw new ArgumentException("Ongeldig e-mailadres.");

            // Haal de zakelijke beheerder op
            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(bedrijfsId);
            if (huurder == null)
                throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden");

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
                bedrijfsMedewerkerId = Guid.NewGuid(),
                medewerkerNaam = medewerkerNaam,
                medewerkerEmail = medewerkerEmail,
                zakelijkeHuurderId = huurder.zakelijkeId
            };

            // Voeg de medewerker toe
            huurder.Medewerkers.Add(medewerker);

            // Update de beheerder in de repository
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
        public void VerwerkPayAsYouGoBetaling(Guid beheerderId, decimal bedrag)
        {
            var huurder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
            if (huurder == null)
                throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden.");

            if (huurder.AbonnementType != AbonnementType.PayAsYouGo)
                throw new InvalidOperationException("Eenmalige betaling is alleen toegestaan voor Pay-As-You-Go abonnementen.");

            // Hier voeg je de logica toe voor eenmalige betaling, zoals het registreren van de transactie.
            string bericht = $@"
    Beste {huurder.beheerderNaam},

    Uw eenmalige betaling van €{bedrag:F2} is succesvol verwerkt.

    Datum: {DateTime.Now:dd-MM-yyyy}

    Met vriendelijke groet,
    Het Team";

            _emailService.SendEmail(huurder.bedrijfsEmail, "Betaling bevestigd", bericht);
        }


        /// <summary>
        /// Verwerkt een betaling voor een prepaid abonnement.
        /// </summary>
        public void VerwerkPrepaidBetaling(WagenparkBeheerder beheerder, decimal kosten)
        {
            if (beheerder == null) throw new ArgumentNullException(nameof(beheerder));

            if (beheerder.PrepaidSaldo < kosten)
                throw new InvalidOperationException("Onvoldoende saldo om deze transactie te voltooien.");

            beheerder.PrepaidSaldo -= kosten;
            _wagenparkBeheerderRepository.UpdateWagenparkBeheerder(beheerder);
            _zakelijkeHuurderRepository.Save();

            string bericht = $@"
            Beste {beheerder.beheerderNaam},

            Uw prepaid saldo is aangepast.
            Bedrag afgetrokken: €{kosten:F2}
            Nieuw saldo: €{beheerder.PrepaidSaldo:F2}

            Datum: {DateTime.Now:dd-MM-yyyy}

            Met vriendelijke groet,
            CarAndAll";

            _emailService.SendEmail(beheerder.bedrijfsEmail, "Saldo update voor uw prepaid-account", bericht);
        }

        /// <summary>
        /// Laadt prepaid saldo op voor een zakelijke beheerder.
        /// </summary>
        public void LaadPrepaidSaldoOp(Guid beheerderId, decimal bedrag)
        {
            var beheerder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
            if (beheerder == null)
                throw new KeyNotFoundException("Zakelijke beheerder niet gevonden.");

            beheerder.PrepaidSaldo += bedrag;
            _wagenparkBeheerderRepository.UpdateWagenparkBeheerder(beheerder);
            _wagenparkBeheerderRepository.Save();

            string bericht = $@"
            Beste {beheerder.beheerderNaam},

            Uw prepaid saldo is succesvol opgewaardeerd.
            Bedrag toegevoegd: €{bedrag:F2}
            Nieuw saldo: €{beheerder.PrepaidSaldo:F2}

            Datum: {DateTime.Now:dd-MM-yyyy}

            Met vriendelijke groet,
            CarAndAll";

            _emailService.SendEmail(beheerder.bedrijfsEmail, "Prepaid saldo opgewaardeerd", bericht);
        }

        /// <summary>
        /// Wijzigt het abonnement van een zakelijke beheerder.
        /// </summary>
        public void WijzigAbonnement(Guid beheerderId, Guid abonnementId, AbonnementType abonnementType)
        {
            var beheerder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
            if (beheerder == null)
                throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden");

            var nieuwAbonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (nieuwAbonnement == null)
                throw new KeyNotFoundException("Abonnement niet gevonden.");

            // Controleer of het abonnementstype overeenkomt
            if (nieuwAbonnement.AbonnementType != abonnementType)               
                throw new InvalidOperationException("Abonnementstype komt niet overeen met het geselecteerde type.");

            if (beheerder.HuidigAbonnement != null)
            {
                var huidigAbonnement = beheerder.HuidigAbonnement;
                huidigAbonnement.WagenparkBeheerders.Remove(beheerder);
                _abonnementRepository.UpdateAbonnement(huidigAbonnement);
            }

            beheerder.HuidigAbonnement = nieuwAbonnement;
            beheerder.AbonnementId = nieuwAbonnement.AbonnementId;
            beheerder.AbonnementType = abonnementType; // Stel het type abonnement in
            beheerder.updateDatumAbonnement = DateTime.Now;

            nieuwAbonnement.WagenparkBeheerders.Add(beheerder);

            _abonnementRepository.UpdateAbonnement(nieuwAbonnement);
            _wagenparkBeheerderRepository.UpdateWagenparkBeheerder(beheerder);
            _wagenparkBeheerderRepository.Save();

            StuurBevestigingsEmail(beheerder, nieuwAbonnement);
        }


        /// <summary>
        /// Bereken de startdatum van de volgende abonnementsperiode.
        /// </summary>
        public DateTime BerekenVolgendePeriode()
        {
            var huidigeDatum = DateTime.Now;
            return new DateTime(huidigeDatum.Year, huidigeDatum.Month, 1).AddMonths(1);
        }

        private void StuurBevestigingsEmail(WagenparkBeheerder beheerder, Abonnement nieuwAbonnement)
        {
            if (beheerder == null || nieuwAbonnement == null) throw new ArgumentNullException();

            string subject = "Bevestiging van uw abonnementswijziging";
            string body = $@"
            Beste {beheerder.beheerderNaam},

            Uw abonnementswijziging is geregistreerd.

            Nieuw abonnement: {nieuwAbonnement.Naam}
            Kosten: €{nieuwAbonnement.Kosten:F2} per maand
            Ingangsdatum: {beheerder.updateDatumAbonnement:dd-MM-yyyy}

            Met vriendelijke groet,
            CarAndAll";

            _emailService.SendEmail(beheerder.bedrijfsEmail, subject, body);
        }
    }
}
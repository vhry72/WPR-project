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

        public IEnumerable<Abonnement> GetAllAbonnementen()
        {
            return _abonnementRepository.GetAllAbonnementen();
        }
        public IEnumerable<Abonnement>GetBijnaVerlopenAbonementen() 
        {
            return _abonnementRepository.GetBijnaVerlopenAbonnementen();
        }

        public void VoegMedewerkerToe(Guid bedrijfsId, string medewerkerNaam, string medewerkerEmail)
        {
            if (string.IsNullOrWhiteSpace(medewerkerEmail) || !medewerkerEmail.Contains("@"))
                throw new ArgumentException("Ongeldig e-mailadres.");

            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(bedrijfsId);
            if (huurder == null)
                throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden");

            var bedrijfDomein = huurder.domein;
            var medewerkerDomein = medewerkerEmail.Split('@')[1];

            if (!bedrijfDomein.Equals(medewerkerDomein, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Medewerker e-mailadres moet het domein {bedrijfDomein} bevatten.");

            if (huurder.Medewerkers.Exists(m => m.medewerkerEmail == medewerkerEmail))
                throw new InvalidOperationException("Deze medewerker is al toegevoegd.");

            var medewerker = new BedrijfsMedewerkers
            {
                bedrijfsMedewerkerId = Guid.NewGuid(),
                medewerkerNaam = medewerkerNaam,
                medewerkerEmail = medewerkerEmail,
                zakelijkeId = huurder.zakelijkeId
            };

            huurder.Medewerkers.Add(medewerker);

            _zakelijkeHuurderRepository.UpdateZakelijkHuurder(huurder);
            _zakelijkeHuurderRepository.Save();

            string bericht = $@"
         Beste {medewerkerNaam},
 
         U bent toegevoegd als medewerker bij het bedrijfsabonnement van {huurder.bedrijfsNaam}.
        U kunt nu gebruik maken van de voordelen van dit abonnement.";

            _emailService.SendEmail(medewerkerEmail, "Welkom bij het bedrijfsabonnement", bericht);
        }

        public void VerwijderMedewerker(Guid bedrijfsId, Guid medewerkerId)
        {
            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(bedrijfsId);
            if (huurder == null)
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

            var medewerker = huurder.Medewerkers.FirstOrDefault(m => m.bedrijfsMedewerkerId == medewerkerId);
            if (medewerker == null)
                throw new KeyNotFoundException("Medewerker niet gevonden.");

            // Verwijder de medewerker
            huurder.Medewerkers.Remove(medewerker);
            _zakelijkeHuurderRepository.UpdateZakelijkHuurder(huurder);
            _zakelijkeHuurderRepository.Save();

            // Bevestiging via e-mail sturen
            string bericht = $"Beste {medewerker.medewerkerNaam},\n\nU bent verwijderd uit het bedrijfsabonnement van {huurder.bedrijfsNaam}.";
            _emailService.SendEmail(medewerker.medewerkerEmail, "Verwijdering uit bedrijfsabonnement", bericht);
        }


        public void VerwerkPayAsYouGoBetaling(Guid beheerderId, decimal bedrag)
        {
            var huurder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
            if (huurder == null)
                throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden.");

            if (huurder.AbonnementType != AbonnementType.PayAsYouGo)
                throw new InvalidOperationException("Eenmalige betaling is alleen toegestaan voor Pay-As-You-Go abonnementen.");

            string bericht = $@"
            Beste {huurder.beheerderNaam},

            Uw eenmalige betaling van €{bedrag:F2} is succesvol verwerkt.

            Datum: {DateTime.Now:dd-MM-yyyy}

            Met vriendelijke groet,
            Het Team";

            _emailService.SendEmail(huurder.bedrijfsEmail, "Betaling bevestigd", bericht);
        }

        public void VerwerkPrepaidBetaling(Guid beheerderId, decimal kosten)
        {
            var huurder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
            if (huurder == null)
                throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden.");

            if (huurder.PrepaidSaldo < kosten)
                throw new InvalidOperationException("Onvoldoende saldo om deze transactie te voltooien.");

            huurder.PrepaidSaldo -= kosten;
            _wagenparkBeheerderRepository.UpdateWagenparkBeheerder(huurder);
            _zakelijkeHuurderRepository.Save();

            string bericht = $@"
        Beste {huurder.beheerderNaam},

        Uw prepaid saldo is aangepast.
        Bedrag afgetrokken: €{kosten:F2}
        Nieuw saldo: €{huurder.PrepaidSaldo:F2}

        Datum: {DateTime.Now:dd-MM-yyyy}

        Met vriendelijke groet,
        CarAndAll";

            _emailService.SendEmail(huurder.bedrijfsEmail, "Saldo update voor uw prepaid-account", bericht);
        }
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
        Nieuwe saldo: €{beheerder.PrepaidSaldo:F2}

        Datum: {DateTime.Now:dd-MM-yyyy}

        Met vriendelijke groet,
        CarAndAll";

            _emailService.SendEmail(beheerder.bedrijfsEmail, "Prepaid saldo opgewaardeerd", bericht);
        }

        public void WijzigAbonnement(Guid beheerderId, Guid abonnementId, AbonnementType abonnementType)
        {
            var beheerder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
            if (beheerder == null)
                throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden");

            var nieuwAbonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (nieuwAbonnement == null)
                throw new KeyNotFoundException("Abonnement niet gevonden.");

            if (nieuwAbonnement.AbonnementType != abonnementType)
                throw new InvalidOperationException("Abonnementstype komt niet overeen met het geselecteerde type");

            if (beheerder.HuidigAbonnement != null)
            {
                var huidigAbonnement = beheerder.HuidigAbonnement;
                huidigAbonnement.WagenparkBeheerders = null;
                _abonnementRepository.UpdateAbonnement(huidigAbonnement);
            }

            beheerder.HuidigAbonnement = nieuwAbonnement;
            beheerder.AbonnementId = nieuwAbonnement.AbonnementId;
            beheerder.AbonnementType = abonnementType;
            beheerder.updateDatumAbonnement = DateTime.Now;

            _abonnementRepository.UpdateAbonnement(nieuwAbonnement);
            _wagenparkBeheerderRepository.UpdateWagenparkBeheerder(beheerder);
            _wagenparkBeheerderRepository.Save();

            StuurBevestigingsEmail(beheerderId, abonnementId);
            StuurFactuurEmail(beheerderId, abonnementId);
        }

        public DateTime BerekenVolgendePeriode()
        {
            var huidigeDatum = DateTime.Now;
            return new DateTime(huidigeDatum.Year, huidigeDatum.Month, 1).AddMonths(1);
        }

        public void WijzigAbonnementMetDirecteKosten(Guid beheerderId, Guid abonnementId, AbonnementType abonnementType)
        {
            var beheerder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
            if (beheerder == null) throw new KeyNotFoundException("Beheerder niet gevonden.");

            var abonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (abonnement == null) throw new KeyNotFoundException("Abonnement niet gevonden.");

            beheerder.HuidigAbonnement = abonnement;
            beheerder.AbonnementType = abonnementType;
            beheerder.updateDatumAbonnement = DateTime.Now;

            _wagenparkBeheerderRepository.UpdateWagenparkBeheerder(beheerder);
            _wagenparkBeheerderRepository.Save();
        }

        public void WijzigAbonnementVanafVolgendePeriode(Guid beheerderId, Guid abonnementId, AbonnementType abonnementType)
        {
            var beheerder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
            if (beheerder == null) throw new KeyNotFoundException("Beheerder niet gevonden.");

            var abonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (abonnement == null) throw new KeyNotFoundException("Abonnement niet gevonden.");

            beheerder.HuidigAbonnement = abonnement;
            beheerder.AbonnementType = abonnementType;

            beheerder.updateDatumAbonnement = BerekenVolgendePeriode();

            _wagenparkBeheerderRepository.UpdateWagenparkBeheerder(beheerder);
            _wagenparkBeheerderRepository.Save();
        }

        public void StuurFactuurEmail(Guid beheerderId, Guid abonnementId)
        {
            var beheerder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
            if (beheerder == null)
                throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden.");
            var abonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (abonnement == null)
                throw new KeyNotFoundException("Abonnement niet gevonden.");

            string subject = "Factuur voor uw nieuwe abonnement";
            string body = $@"
            Beste {beheerder.beheerderNaam},

            Hartelijk dank voor het kiezen van het {abonnement.Naam}-abonnement.
            Hieronder vindt u de details van uw abonnement:

            Abonnement: {abonnement.Naam}
            Kosten: €{abonnement.Kosten:F2} per maand
            Startdatum: {beheerder.updateDatumAbonnement:dd-MM-yyyy}

            Bewaar deze factuur voor uw administratie.
            Met vriendelijke groet,
            CarAndAll";

            _emailService.SendEmail(beheerder.bedrijfsEmail, subject, body);
        }
        public void StuurBevestigingsEmail(Guid beheerderId, Guid abonnementId)
        {
            // Haal de WagenparkBeheerder en Abonnement op
            var beheerder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
               if (beheerder == null) {
                throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden.");
            }
            var abonnement = _abonnementRepository.GetAbonnementById(abonnementId);
               if (abonnement == null) {
                throw new KeyNotFoundException("Abonnement niet gevonden.");
            }
            // Factuur bericht
            string subject = "Bevestiging van uw abonnementswijziging";
            string body = $@"
            Beste {beheerder.beheerderNaam},

            Uw abonnementswijziging is geregistreerd.

            Nieuw abonnement: {abonnement.Naam}
            Kosten: €{abonnement.Kosten:F2} per maand
            Ingangsdatum: {beheerder.updateDatumAbonnement:dd-MM-yyyy}

            Met vriendelijke groet,
            CarAndAll";

            // Verstuur de e-mail
            _emailService.SendEmail(beheerder.bedrijfsEmail, subject, body);
        }
        public Abonnement GetAbonnementDetails(Guid abonnementId)
        {
            var abonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (abonnement == null)
                throw new KeyNotFoundException("Abonnement niet gevonden.");

            // Bereken het aantal dagen tussen de begin- en vervaldatum
            int aantalDagen = (abonnement.vervaldatum - abonnement.beginDatum).Days;

            // Update het abonnement object met de berekende waarde
            abonnement.AantalDagen = aantalDagen;
            abonnement.korting = 10m;
            abonnement.details = $"Dit abonnement bevat extra voordelen voor bedrijfsgebruikers: {abonnement.Naam}.";
            abonnement.directZichtbaar = false;

            // Sla de wijzigingen op in de database
            _abonnementRepository.UpdateAbonnement(abonnement);
            _abonnementRepository.Save();

            // Retourneer het bijgewerkte abonnement
            return abonnement;
        }
    }
}
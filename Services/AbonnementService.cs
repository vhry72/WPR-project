using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    //dit is de service voor het abonnement
    public class AbonnementService
    {
        private readonly IAbonnementRepository _abonnementRepository;
        private readonly IWagenparkBeheerderRepository _wagenparkBeheerderRepository;
        private readonly IEmailService _emailService;
        private readonly FactuurService _factuurService;

        public AbonnementService(
            IAbonnementRepository abonnementRepository,
            IWagenparkBeheerderRepository wagenparkBeheerderRepository,
            IEmailService emailService,
            FactuurService factuurService)
        {
            _abonnementRepository = abonnementRepository;
            _wagenparkBeheerderRepository = wagenparkBeheerderRepository;
            _emailService = emailService;
            _factuurService = factuurService;
        }

        public IEnumerable<Abonnement> GetAllAbonnementen()
        {
            return _abonnementRepository.GetAllAbonnementen();
        }


        public Abonnement GetAbonnementById(Guid id)
        {
            return _abonnementRepository.GetAbonnementById(id);
        }

        public void VoegMedewerkerToe(Guid beheerderId, Guid medewerkerId)
        {
            // Haal de lijst van medewerkers op die onder deze medewerkerVanBeheerder vallen
            var medewerkersVanBeheerder = _wagenparkBeheerderRepository.GetMedewerkersByWagenparkbeheerder(beheerderId);

            // Controleer of de opgegeven medewerker bij deze medewerkerVanBeheerder hoort
            var medewerker = medewerkersVanBeheerder.FirstOrDefault(m => m.bedrijfsMedewerkerId.Equals(medewerkerId));
            if (medewerker == null)
            {
                throw new InvalidOperationException("Deze medewerker zit niet in uw wagenpark.");
            }

            // Controleer of de medewerker al een abonnement heeft
            if (medewerker.AbonnementId.HasValue)
            {
                throw new InvalidOperationException("Deze medewerker heeft al een abonnement.");
            }

            // Haal het AbonnementId op dat bij deze medewerkerVanBeheerder hoort
            var abonnementIdVanBeheerder = _wagenparkBeheerderRepository.GetAbonnementId(beheerderId);
            if (abonnementIdVanBeheerder == null)
            {
                throw new InvalidOperationException("Er is geen abonnement gekoppeld aan deze medewerkerVanBeheerder.");
            }

            // Koppel het abonnement aan de medewerker
            medewerker.AbonnementId = abonnementIdVanBeheerder;

            // Sla de wijzigingen op
            _wagenparkBeheerderRepository.Save();

            string bericht = $@"
         Beste {medewerker.medewerkerNaam},

         U bent toegevoegd als medewerker bij het bedrijfsabonnement.
        U kunt nu gebruik maken van de voordelen van dit abonnement.";

            _emailService.SendEmail(medewerker.medewerkerEmail, "Welkom bij het bedrijfsabonnement", bericht);
        }

        public void VerwijderMedewerker(Guid beheerderId, Guid medewerkerId)
        {
            // Haal de lijst van medewerkers op die onder deze WagenparkBeheerder vallen
            var medewerkersVanBeheerder = _wagenparkBeheerderRepository.GetMedewerkersByWagenparkbeheerder(beheerderId);

            // Controleer of de opgegeven medewerker bij deze WagenparkBeheerder hoort
            var medewerker = medewerkersVanBeheerder.FirstOrDefault(m => m.bedrijfsMedewerkerId.Equals(medewerkerId));
            if (medewerker == null)
            {
                throw new KeyNotFoundException("Deze medewerker zit niet in uw wagenpark.");
            }

            // Controleer of de medewerker een abonnement heeft
            if (medewerker.AbonnementId.HasValue)
            {
                // Ontkoppel het abonnement van de medewerker
                medewerker.AbonnementId = null;
            }

            // Verwijder de medewerker uit de lijst
            medewerkersVanBeheerder.Remove(medewerker);

            // Sla de wijzigingen op
            _wagenparkBeheerderRepository.Save();

            // Bevestiging via e-mail sturen
            string bericht = $"Beste {medewerker.medewerkerNaam},\n\nU bent verwijderd uit het bedrijfsabonnement.";
            _emailService.SendEmail(medewerker.medewerkerEmail, "Verwijderd uit bedrijfsabonnement", bericht);
        }


        // Voeg een nieuw abonnement toe
        public void AddAbonnement(Abonnement abonnement, Guid beheerderId)

        {
            if (abonnement == null)
            {
                throw new ArgumentNullException(nameof(abonnement), "Abonnement mag niet null zijn.");
            }
            
            
            _abonnementRepository.AddAbonnement(abonnement);
            _abonnementRepository.Save();
            StuurFactuur(beheerderId, abonnement.AbonnementId);
        }


        // Laad prepaid saldo op voor een wagenparkbeheerder

        public void LaadPrepaidSaldoOp(Guid beheerderId, decimal bedrag)
        {
            var beheerder = _wagenparkBeheerderRepository.GetBeheerderById(beheerderId);
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



        public void WijzigAbonnementVanafVolgendePeriode(AbonnementWijzigDTO dto)
        {
            try
            {
                _abonnementRepository.UpdateInToekomst(dto);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        // Haal de details van een abonnement op
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

        public void StuurFactuur(Guid beheerderId, Guid abonnementId)
        {
            try
            {
                // Genereer de PDF
                var pdfBytes = _factuurService.GenerateInvoicePDF(beheerderId, abonnementId);
                var beheerder = _wagenparkBeheerderRepository.GetBeheerderById(beheerderId);
                if (beheerder == null)
                    throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden.");

                // Stuur de e-mail met de PDF als bijlage
                string subject = "Factuur voor uw nieuwe abonnement";
                string body = "Bijgevoegd vindt u uw factuur.";
                _emailService.SendEmailWithAttachment(beheerder.bedrijfsEmail, subject, body, pdfBytes, "Factuur.pdf");
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
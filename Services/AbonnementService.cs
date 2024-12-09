//using System;
//using System.Collections.Generic;
//using System.Linq;
//using WPR_project.Models;
//using WPR_project.Repositories;
//using WPR_project.Services.Email;

//namespace WPR_project.Services
//{
//    public class AbonnementService
//    {
//        private readonly IAbonnementRepository _abonnementRepository;
//        private readonly IZakelijkeHuurderRepository _zakelijkeHuurderRepository;
//        private readonly IEmailService _emailService;

//        public AbonnementService(
//            IAbonnementRepository abonnementRepository,
//            IZakelijkeHuurderRepository zakelijkeHuurderRepository,
//            IEmailService emailService)
//        {
//            _abonnementRepository = abonnementRepository;
//            _zakelijkeHuurderRepository = zakelijkeHuurderRepository;
//            _emailService = emailService;
//        }

//        public IEnumerable<Abonnement> GetAllAbonnementen()
//        {
//            return _abonnementRepository.GetAllAbonnementen();
//        }

//        public void WijzigAbonnement(Guid zakelijkeId, Guid nieuwAbonnementId)
//        {
//            var huurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeId);
//            if (huurder == null)
//                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");

//            var nieuwAbonnement = _abonnementRepository.GetAbonnementById(nieuwAbonnementId);
//            if (nieuwAbonnement == null)
//                throw new KeyNotFoundException("Abonnement niet gevonden.");

//            // Toewijzing van het nieuwe abonnement
//            huurder.NieuwAbonnementId = nieuwAbonnementId;
//            huurder.IngangsdatumNieuwAbonnement = BerekenVolgendePeriode(); // Stel de startdatum in

//            _zakelijkeHuurderRepository.UpdateZakelijkHuurder(huurder);
//            _zakelijkeHuurderRepository.Save();

//            // Stuur een bevestigingsmail
//            StuurBevestigingsEmail(huurder, nieuwAbonnement);
//        }
        
//        public DateTime BerekenVolgendePeriode()
//        {
//            // Stel de startdatum van de volgende abonnementsperiode in
//            var huidigeDatum = DateTime.Now;
//            return new DateTime(huidigeDatum.Year, huidigeDatum.Month, 1).AddMonths(1);
//        }

//        private void StuurBevestigingsEmail(ZakelijkHuurder huurder, Abonnement nieuwAbonnement)
//        {
//            string subject = "Bevestiging van uw abonnementswijziging";
//            string body = $@"
//            Beste {huurder.bedrijfsNaam},<br><br>
//            Uw abonnementswijziging is geregistreerd.<br><br>
//            Nieuw abonnement: <strong>{nieuwAbonnement.Naam}</strong><br>
//            Kosten: €{nieuwAbonnement.Kosten:F2} per maand<br>
//            De wijziging wordt actief op: {huurder.IngangsdatumNieuwAbonnement:dd-MM-yyyy}.<br><br>
//            Met vriendelijke groet,<br>
//            Het WPR Project Team
//        ";

//            _emailService.SendEmail(huurder.email, subject, body);
//        }
//    }
//}


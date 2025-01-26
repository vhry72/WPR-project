using Microsoft.AspNetCore.Mvc;
using WPR_project.Services;
using WPR_project.DTO_s;
using WPR_project.Services.Email;

namespace WPR_project.Controllers
// dit is de controller voor de abonnementen van de wagenparkbeheerder en de zakelijke huurder
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbonnementController : ControllerBase
    {
        private readonly AbonnementService _service;
        private readonly WagenparkBeheerderService _WagenparkService;
        private readonly ZakelijkeHuurderService _zakelijkeHuurderService;
        private readonly FactuurService _factuurService;
        private readonly EmailService _emailService;


        public AbonnementController(
            AbonnementService service,
            WagenparkBeheerderService WagenparkbeheerderService,
            ZakelijkeHuurderService zakelijkeHuurderService,
            FactuurService factuurService,
            EmailService emailService)
        {
            _service = service;
            _WagenparkService = WagenparkbeheerderService;
            _zakelijkeHuurderService = zakelijkeHuurderService;
            _factuurService = factuurService;
            _emailService = emailService;
        }

        // Hier vraag je alle abonnementen op
        [HttpGet]
        public IActionResult GetAllAbonnementen()
        {
            try
            {
                var abonnementen = _service.GetAllAbonnementen();
                return Ok(abonnementen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }


        // Hier maak je een abonnement aan voor de zakelijke huurder
        [HttpPost("{beheerderId}/abonnement/maken")]
        public IActionResult MaakBedrijfsAbonnement(Guid beheerderId, [FromBody] AbonnementDTO abonnementDto)
        {
            // Controleer of het verzoek geldig is
            if (abonnementDto == null)
            {
                return BadRequest(new { Error = "Het verzoek mag niet null zijn." });
            }

            if (string.IsNullOrEmpty(abonnementDto.Naam) || abonnementDto.Kosten <= 0)
            {
                return BadRequest(new { Error = "Een geldig abonnementnaam en kosten zijn vereist." });
            }

            if (abonnementDto.zakelijkeId == Guid.Empty)
            {
                return BadRequest(new { Error = "Een geldig zakelijkeId is vereist." });
            }

            try
            {
                // Controleer of de beheerder bestaat
                var beheerder = _WagenparkService.GetBeheerderById(beheerderId);
                if (beheerder == null)
                {
                    return NotFound(new { Error = "Wagenparkbeheerder niet gevonden." });
                }

                // Controleer of de zakelijke ID geldig is
                var zakelijkeHuurder = _zakelijkeHuurderService.GetZakelijkHuurderById(abonnementDto.zakelijkeId);
                if (zakelijkeHuurder == null)
                {
                    return NotFound(new { Error = "Zakelijke huurder niet gevonden." });
                }

                // Maak een nieuw abonnement aan met de gegeven gegevens
                var nieuwAbonnement = new Abonnement
                {
                    AbonnementId = Guid.NewGuid(),
                    Naam = abonnementDto.Naam,
                    beginDatum = DateTime.Now,
                    vervaldatum = DateTime.Now.AddYears(1),
                    Kosten = abonnementDto.Kosten,
                    AbonnementTermijnen = abonnementDto.AbonnementTermijnen,
                    AbonnementType = abonnementDto.Type,
                    zakelijkeId = abonnementDto.zakelijkeId,
                    korting = abonnementDto.korting,
                    IsActive = true,
                    status = false

                };

                // Voeg het abonnement toe via de service
                _service.AddAbonnement(nieuwAbonnement);

                // Beheerder koppelen aan abonnement
                _WagenparkService.UpdateWagenParkBeheerderAbonnement(beheerderId, nieuwAbonnement.AbonnementId);

                return Ok(new { Message = "Bedrijfsabonnement succesvol aangemaakt.", AbonnementId = nieuwAbonnement.AbonnementId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Er is een interne fout opgetreden.", Details = ex.Message });
            }
        }


        // Hier kun je het saldo van een zakelijke huurder opwaarderen
        [HttpPost("{beheerderId}/saldo/opwaarderen")]
        public IActionResult LaadSaldoOp(Guid zakelijkeId, [FromBody] decimal bedrag)
        {
            try
            {
                // Laad het saldo van een zakelijke huurder op met een specifiek bedrag
                _service.LaadPrepaidSaldoOp(zakelijkeId, bedrag);
                return Ok(new { Message = "Saldo succesvol opgewaardeerd." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // Hier voeg je een medewerker toe aan een abonnement
        [HttpPost("{beheerderId}/medewerker/toevoegen/{medewerkerId}")]
        public IActionResult VoegMedewerkerToe(Guid beheerderId, Guid medewerkerId)
        {
           
            try
            {
                // Voeg de medewerker toe aan het abonnement
                _service.VoegMedewerkerToe(beheerderId, medewerkerId);
                return Ok(new { Message = "Medewerker succesvol toegevoegd." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Hier verwijder je een medewerker van een abonnement
        [HttpDelete("{beheerderId}/medewerker/verwijderen/{medewerkerId}")]
        public IActionResult VerwijderMedewerker(Guid beheerderId, Guid medewerkerId)
        {
            try
            {
                _service.VerwijderMedewerker(beheerderId, medewerkerId);
                return Ok(new { Message = "Medewerker succesvol verwijderd." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Er is een interne fout opgetreden.", Details = ex.Message });
            }
        }


        // Hier haal je het huidige abonnement van een wagenparkbeheerder op
        [HttpGet("{beheerderId}/huidig-abonnement")]
        public IActionResult GetHuidigAbonnement(Guid beheerderId)
        {
            try
            {
                // Haal de details van het huidige abonnement van een wagenparkbeheerder op
                var abonnement = _service.GetAbonnementDetails(beheerderId);
                return Ok(abonnement);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Er is een interne fout opgetreden.", Details = ex.Message });
            }
        }

        // Hier kun je een bestaand abonnement wijzigen
        [HttpPost("{beheerderId}/abonnement/wijzig")]
        public IActionResult WijzigAbonnement(Guid beheerderId, [FromBody] AbonnementWijzigDTO abonnement)
        {
            if (abonnement == null)
            {
                return BadRequest(new { Error = "Het verzoek mag niet leeg zijn." });
            }

            if (abonnement.AbonnementId == Guid.Empty)
            {
                return BadRequest(new { Error = "Een geldig abonnementId is vereist." });
            }

            try
            {
                // Controle of directZichtbaar of volgendePeriode correct zijn ingesteld
                if (abonnement.directZichtbaar == true)
                {
                    _service.WijzigAbonnementMetDirecteKosten(beheerderId, abonnement.AbonnementId, abonnement.AbonnementType);
                }
                else if (abonnement.AantalDagen.HasValue && abonnement.AantalDagen > 0)
                {
                    _service.WijzigAbonnementVanafVolgendePeriode(beheerderId, abonnement.AbonnementId, abonnement.AbonnementType);
                }
                else
                {
                    return BadRequest(new { Error = "Geef aan of de wijziging direct zichtbaar moet zijn of vanaf de volgende periode moet ingaan." });
                }

                return Ok(new { Message = "Abonnement succesvol gewijzigd." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Er is een interne fout opgetreden.", Details = ex.Message });
            }
        }

        [HttpPost("{beheerderId}/factuur/stuur")]
        public IActionResult StuurFactuur(Guid beheerderId, [FromBody] Guid abonnementId)
        {
            try
            {
                // Genereer de PDF
                var pdfBytes = _factuurService.GenerateInvoicePDF(beheerderId, abonnementId);
                var beheerder = _WagenparkService.GetBeheerderById(beheerderId);
                if (beheerder == null)
                    throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden.");

                // Stuur de e-mail met de PDF als bijlage
                string subject = "Factuur voor uw nieuwe abonnement";
                string body = "Bijgevoegd vindt u uw factuur.";
                _emailService.SendEmailWithAttachment(beheerder.bedrijfsEmail, subject, body, pdfBytes, "Factuur.pdf");

                return Ok(new { Message = "Factuur succesvol verstuurd." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
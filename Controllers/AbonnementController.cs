using Microsoft.AspNetCore.Mvc;
using System;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.DTO_s;
using Microsoft.EntityFrameworkCore;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Controllers
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

        // Haalt alle beschikbare abonnementen op.
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

        //haalt alle abonnnementen op die binnen een maand verlopen
        [HttpGet("bijna-verlopen")]
        public IActionResult GetBijnaVerlopenAbonnementen()
        {
            try
            {
                // hier moet je service.getabbodetails voegen en bereken vanaf een limiet van 30 dagen.
                var abonnementen = _service.GetAllAbonnementen();
                return Ok(abonnementen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }
        //Geeft abonnement details terug
        [HttpGet("{abonnementId}/details")]
        public IActionResult GetAbonnementDetails(Guid abonnementId)
        {
            try
            {
                var abonnementDetails = _service.GetAbonnementDetails(abonnementId);
                return Ok(abonnementDetails);
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

        [HttpPost("{beheerderId}/abonnement/maken")]
        public IActionResult MaakBedrijfsAbonnement(Guid beheerderId, [FromBody] AbonnementDTO abonnementDto)
        {
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

                // Maak een nieuw abonnement aan
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
                    korting = abonnementDto.korting
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



        //hier moet nog een Get endpoint komen om het abonnement van het bedrijf te weergeven


        [HttpPost("{beheerderId}/prepaid/betalen")]
        public IActionResult VerwerkPrepaidBetaling(Guid beheerderId, [FromBody] decimal kosten)
        {
            try
            {
                _service.VerwerkPrepaidBetaling(beheerderId, kosten);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("{beheerderId}/saldo/opwaarderen")]
        public IActionResult LaadSaldoOp(Guid zakelijkeId, [FromBody] decimal bedrag)
        {
            try
            {
                _service.LaadPrepaidSaldoOp(zakelijkeId, bedrag);
                return Ok(new { Message = "Saldo succesvol opgewaardeerd." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{beheerderId}/eenmalig/betalen")]
        public IActionResult EenmaligeBetaling(Guid beheerderId, [FromBody] decimal bedrag)
        {
            try
            {
                _service.VerwerkPayAsYouGoBetaling(beheerderId, bedrag);
                return Ok(new { Message = "Betaling succesvol verwerkt." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{beheerderId}/medewerker/toevoegen")]
        public IActionResult VoegMedewerkerToe(Guid zakelijkeId, [FromBody] string medewerkerEmail, string medewerkerNaam)
        {
            try
            {
                _service.VoegMedewerkerToe(zakelijkeId, medewerkerEmail, medewerkerNaam);
                return Ok(new { Message = "Medewerker succesvol toegevoegd." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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


        // haalt de huidige abonnement op van de beheerder
        [HttpGet("{beheerderId}/huidig-abonnement")]
        public IActionResult GetHuidigAbonnement(Guid beheerderId)
        {
            try
            {
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


        [HttpPost("{beheerderId}/bevestiging/stuur")]
        public IActionResult StuurBevestigingsEmail(Guid beheerderId, [FromBody] Guid abonnementId)
        {
            try
            {
                _service.StuurBevestigingsEmail(beheerderId, abonnementId);
                return Ok(new { Message = "Bevestigingsmail succesvol verstuurd." });
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
using Microsoft.AspNetCore.Mvc;
using System;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.DTO_s;
using Microsoft.EntityFrameworkCore;
using WPR_project.Repositories;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbonnementController : ControllerBase
    {
        private readonly AbonnementService _service;
        private readonly WagenparkBeheerderRepository _wagenparkBeheerderRepository;


        public AbonnementController(AbonnementService service, WagenparkBeheerderRepository wagenparkBeheerderRepository)
        {
            _service = service;
            _wagenparkBeheerderRepository = wagenparkBeheerderRepository;
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
        public IActionResult MaakBedrijfsAbonnement(Guid beheerderId, [FromBody] AbonnementDTO abonnementSoort)
        {
            if (abonnementSoort == null)
            {
                return BadRequest(new { Error = "Het verzoek mag niet null zijn." });
            }

            if (abonnementSoort.AbonnementId == Guid.Empty)
            {
                return BadRequest(new { Error = "Een geldig AbonnementId is vereist." });
            }

            try
            {
                // Stap 1: Controleer of de beheerder bestaat
                var beheerder = _wagenparkBeheerderRepository.getBeheerderById(beheerderId);
                if (beheerder == null)
                {
                    return NotFound(new { Error = "Wagenparkbeheerder niet gevonden." });
                }

                // Stap 2: Verwerk de betalingsmethode en wijzig het abonnement
                if (abonnementSoort.Type == AbonnementType.PayAsYouGo)
                {
                    _service.VerwerkPayAsYouGoBetaling(beheerderId, abonnementSoort.Kosten);
                }
                else if (abonnementSoort.Type == AbonnementType.PrepaidSaldo)
                {
                    _service.LaadPrepaidSaldoOp(beheerderId, abonnementSoort.Kosten);
                }
                else
                {
                    return BadRequest(new { Error = "Ongeldige betalingsmethode." });
                }

                // Stap 3: Wijzig het abonnement met de juiste voorwaarden
                if (abonnementSoort.directZichtbaar == true)
                {
                    _service.WijzigAbonnementMetDirecteKosten(beheerderId, abonnementSoort.AbonnementId, abonnementSoort.Type);
                }
                else
                {
                    _service.WijzigAbonnementVanafVolgendePeriode(beheerderId, abonnementSoort.AbonnementId, abonnementSoort.Type);
                }

                // Stap 4: Bevestigingsmails verzenden
                _service.StuurBevestigingsEmail(beheerderId, abonnementSoort.AbonnementId);
                _service.StuurFactuurEmail(beheerderId, abonnementSoort.AbonnementId);

                return Ok(new { Message = "Bedrijfsabonnement succesvol aangemaakt en verwerkt." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
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
        public IActionResult WijzigAbonnement(Guid beheerderId, [FromBody] Abonnement abonnement)
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
                _service.StuurFactuurEmail(beheerderId, abonnementId);
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
using Microsoft.AspNetCore.Mvc;
using System;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.DTO_s;
using Microsoft.EntityFrameworkCore;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbonnementController : ControllerBase
    {
        private readonly AbonnementService _service;


        public AbonnementController(AbonnementService service)
        {
            _service = service;
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
        public IActionResult MaakBedrijfsAbonnement(Guid zakelijkeId, [FromBody] AbonnementDTO abonnementSoort)
        {
            try
            {
                // Stap 2: Verwerk de betalingsmethode
                if (abonnementSoort.Type == AbonnementType.PayAsYouGo)
                {
                    _service.VerwerkPayAsYouGoBetaling(zakelijkeId, abonnementSoort.Bedrag);
                }
                else if (abonnementSoort.Type == AbonnementType.PrepaidSaldo)
                {
                    _service.LaadPrepaidSaldoOp(zakelijkeId, abonnementSoort.Bedrag);
                }
                else
                {
                    return BadRequest(new { Message = "Ongeldige betalingsmethode." });
                }

                // Stap 3: Wijzig een abonnement
                _service.WijzigAbonnement(zakelijkeId, abonnementSoort.AbonnementId, abonnementSoort.Type);

                return Ok(new { Message = "Bedrijfsabonnement succesvol aangemaakt." });
            }
            catch (Exception ex)
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

        // Wijzigt een abonnement op basis van het geselecteerde type (direct zichtbaar of vanaf de volgende periode).
        [HttpPost("{beheerderId}/abonnement/wijzig")]
        public IActionResult WijzigAbonnement(Guid beheerderId, [FromBody] AbonnementVerzoek verzoek)
        {
            try
            {
                if (verzoek.directZichtbaar)
                {
                    _service.WijzigAbonnementMetDirecteKosten(beheerderId, verzoek.AbonnementId, verzoek.AbonnementType);
                }
                else if (verzoek.volgendePeriode)
                {
                    _service.WijzigAbonnementVanafVolgendePeriode(beheerderId, verzoek.AbonnementId, verzoek.AbonnementType);
                }
                else
                {
                    return BadRequest(new { Message = "Geef aan of de wijziging direct zichtbaar moet zijn of vanaf de volgende periode moet ingaan." });
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
using Microsoft.AspNetCore.Mvc;
using System;
using WPR_project.Services;

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

        [HttpPost("{zakelijkeId}/saldo/opwaarderen")]
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

        [HttpPost("{zakelijkeId}/eenmalig/betalen")]
        public IActionResult EenmaligeBetaling(Guid zakelijkeId, [FromBody] decimal bedrag)
        {
            try
            {
                _service.VerwerkPayAsYouGoBetaling(zakelijkeId, bedrag);
                return Ok(new { Message = "Betaling succesvol verwerkt." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    

        [HttpPost("{zakelijkeId}/medewerker/toevoegen")]
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


        // Wijzigt het abonnement van een zakelijke huurder
        // name="zakelijkeId">ID van de zakelijke huurder
        // name="nieuwAbonnementId">ID van het nieuwe abonnement
        [HttpPost("{zakelijkeId}/wijzig/{nieuwAbonnementId}")]
        public IActionResult WijzigAbonnement(Guid zakelijkeId, Guid nieuwAbonnementId)
        {
            try
            {
                _service.WijzigAbonnement(zakelijkeId, nieuwAbonnementId);

                // Bereken de ingangsdatum van de wijziging
                var ingangsdatum = _service.BerekenVolgendePeriode();

                return Ok(new
                {
                    Message = "Abonnement succesvol gewijzigd.",
                    NieuwAbonnementId = nieuwAbonnementId,
                    Ingangsdatum = ingangsdatum.ToString("yyyy-MM-dd")
                });
            }
            catch (KeyNotFoundException ex)
            {
                // Specifieke fout voor niet gevonden resources
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Algemene foutafhandeling
                return BadRequest(new
                {
                    Error = "Er is een fout opgetreden bij het wijzigen van het abonnement.",
                    Details = ex.Message
                });
            }
        }
    }
}
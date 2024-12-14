using Microsoft.AspNetCore.Mvc;
using System;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.DTO_s;

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

        [HttpPost("{zakelijkeId}/abonnement/maken")]
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

        [HttpPost("{beheerderId}/abonnement/wijzig")]
        public IActionResult WijzigAbonnement(Guid beheerderId, [FromBody] AbonnementVerzoek verzoek)
        {
            try
            {
                _service.WijzigAbonnement(beheerderId, verzoek.AbonnementId, verzoek.AbonnementType);
                return Ok(new { Message = "Abonnement succesvol gewijzigd." });
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
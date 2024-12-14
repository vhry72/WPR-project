using Microsoft.AspNetCore.Mvc;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.DTO_s;
using WPR_project.Services.Email;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WagenparkBeheerderController : ControllerBase
    {
        private readonly WagenparkBeheerderService _service;
        private readonly EmailService _emailService;

        public WagenparkBeheerderController(WagenparkBeheerderService service)
        {
            _service = service;
        }

        /// <summary>
        /// Haalt alle wagenparkbeheerders op
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<WagenparkBeheerder>> GetAllBeheerders()
        {
            var beheerders = _service.GetWagenparkBeheerders();
            return Ok(beheerders);
        }

        /// <summary>
        /// Haalt een wagenparkbeheerder op via ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<WagenparkBeheerder> GetBeheerderById(Guid id)
        {
            var beheerder = _service.GetBeheerderById(id);
            if (beheerder == null)
            {
                return NotFound(new { Message = "Beheerder niet gevonden." });
            }
            return Ok(beheerder);
        }

        /// <summary>
        /// Voegt een nieuwe wagenparkbeheerder toe
        /// </summary>
        [HttpPost("registerWagenparkDTO")]
        public ActionResult AddBeheerder([FromBody] WagenparkBeheerderDTO beheerderDTO)
        {
            if (beheerderDTO == null || string.IsNullOrEmpty(beheerderDTO.beheerderEmail))
            {
                return BadRequest("Ongeldige gegevens voor registratie");
            }

            var beheerder = new WagenparkBeheerder
            {
                beheerderId = Guid.NewGuid(),
                beheerderNaam = beheerderDTO.beheerderNaam,
                bedrijfsEmail = beheerderDTO.beheerderEmail,
                wachtwoord = beheerderDTO.wachtwoord
            };

            _service.AddWagenparkBeheerder(beheerder);
            return CreatedAtAction(nameof(GetBeheerderById), new { id = beheerder.beheerderId }, beheerder);
        }

        /// <summary>
        /// Wijzigt een bestaande wagenparkbeheerder
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult UpdateBeheerder(Guid id, [FromBody] WagenparkBeheerder beheerder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _service.UpdateWagenparkBeheerder(id, beheerder);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Verwijdert een wagenparkbeheerder via ID
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteBeheerder(Guid id)
        {
            try
            {
                _service.DeleteWagenparkBeheerder(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Voegt een medewerker toe aan een zakelijke huurder
        /// </summary>
        [HttpPost("{zakelijkeId}/voegmedewerker")]
        public IActionResult VoegMedewerkerToe(Guid zakelijkeId, [FromBody] BedrijfsMedewerkers medewerker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Voeg medewerker toe via de service
                _service.VoegMedewerkerToe(zakelijkeId, medewerker.medewerkerNaam, medewerker.medewerkerEmail);

                // Verstuur bevestigingsmail naar de medewerker
                _emailService.SendEmail(
                    medewerker.medewerkerEmail,
                    "Welkom bij het systeem",
                    $"Hallo {medewerker.medewerkerNaam},\n\nJe bent succesvol toegevoegd als medewerker. Gebruik je geregistreerde e-mailadres om in te loggen."
                );

                // Verstuur notificatie naar wagenparkbeheerder
                var beheerderEmail = _service.GetBeheerderEmailById(zakelijkeId); // Zorg dat deze service beschikbaar is
                if (!string.IsNullOrEmpty(beheerderEmail))
                {
                    _emailService.SendEmail(
                        beheerderEmail,
                        "Nieuwe Medewerker Toegevoegd",
                        $"Een nieuwe medewerker ({medewerker.medewerkerNaam}) is toegevoegd aan uw account."
                    );
                }

                return Ok(new { Message = "Medewerker succesvol toegevoegd. Bevestigingsmail verzonden." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Verwijdert een medewerker van een zakelijke huurder
        /// </summary>
        [HttpDelete("{zakelijkeId}/verwijdermedewerker/{medewerkerId}")]
        public IActionResult VerwijderMedewerker(Guid zakelijkeId, Guid medewerkerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                var medewerker = _service.GetMedewerkerById(medewerkerId);
                if (medewerker == null)
                {
                    return NotFound(new { Message = "Medewerker niet gevonden." });
                }

                // Verwijder de medewerker via de service
                _service.VerwijderMedewerker(zakelijkeId, medewerkerId);

                // Verstuur notificatie naar wagenparkbeheerder
                var beheerderEmail = _service.GetBeheerderEmailById(zakelijkeId); // Zorg dat deze service beschikbaar is
                if (!string.IsNullOrEmpty(beheerderEmail))
                {
                    _emailService.SendEmail(
                        beheerderEmail,
                        "Medewerker Verwijderd",
                        $"De medewerker ({medewerker.medewerkerNaam}) is verwijderd van uw account."
                    );
                }

                return Ok(new { Message = "Medewerker succesvol verwijderd." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using WPR_project.Models;
using WPR_project.Services;

namespace WPR_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZakelijkeHuurderController : ControllerBase
    {
        private readonly ZakelijkeHuurderService _service;

        public ZakelijkeHuurderController(ZakelijkeHuurderService service)
        {
            _service = service;
        }

        // POST: api/ZakelijkeHuurder/register
        [HttpPost("register")]
        public ActionResult RegisterZakelijkeHuurder([FromBody] ZakelijkHuurder zakelijkHuurder)
        {
            if (zakelijkHuurder == null || string.IsNullOrEmpty(zakelijkHuurder.email))
            {
                return BadRequest("Ongeldige gegevens voor registratie.");
            }

            _service.RegisterZakelijkeHuurder(zakelijkHuurder);
            return Ok("Registratie succesvol. Controleer je e-mail voor de verificatielink.");
        }

        // GET: api/ZakelijkeHuurder/verify?token={token}
        [HttpGet("verify")]
        public IActionResult VerifyEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Verificatie token is verplicht.");
            }

            var result = _service.VerifyEmail(token);
            if (!result)
            {
                return NotFound("Verificatie mislukt. Token is ongeldig of verlopen.");
            }

            return Ok("Je e-mail is succesvol bevestigd. Je kunt nu inloggen.");
        }

        // GET: api/ZakelijkeHuurder
        [HttpGet]
        public ActionResult<IEnumerable<ZakelijkHuurder>> GetAllZakelijkeHuurders()
        {
            var huurders = _service.GetAllZakelijkHuurders();
            return Ok(huurders);
        }

        // GET: api/ZakelijkeHuurder/{id}
        [HttpGet("{id}")]
        public ActionResult<ZakelijkHuurder> GetZakelijkHuurderById(Guid id)
        {
            var huurder = _service.GetZakelijkHuurderById(id);
            if (huurder == null)
            {
                return NotFound("Zakelijke huurder niet gevonden.");
            }
            return Ok(huurder);
        }

        // POST: api/ZakelijkeHuurder
        [HttpPost]
        public ActionResult AddZakelijkeHuurder([FromBody] ZakelijkHuurder zakelijkHuurder)
        {
            if (zakelijkHuurder == null)
            {
                return BadRequest("Ongeldige gegevens voor huurder.");
            }

            _service.AddZakelijkHuurder(zakelijkHuurder);
            return CreatedAtAction(nameof(GetZakelijkHuurderById), new { id = zakelijkHuurder.zakelijkeId }, zakelijkHuurder);
        }

        // PUT: api/ZakelijkeHuurder/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateZakelijkeHuurder(Guid id, [FromBody] ZakelijkHuurder zakelijkHuurder)
        {
            if (zakelijkHuurder == null || zakelijkHuurder.zakelijkeId != id)
            {
                return BadRequest("ID komt niet overeen met de gegevens.");
            }

            try
            {
                _service.Update(id, zakelijkHuurder);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Zakelijke huurder niet gevonden.");
            }

            return NoContent();
        }

        // DELETE: api/ZakelijkeHuurder/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteZakelijkeHuurder(Guid id)
        {
            try
            {
                _service.Delete(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Zakelijke huurder niet gevonden.");
            }

            return NoContent();
        }

        // POST: api/ZakelijkeHuurder/{id}/voegmedewerker
        [HttpPost("{id}/voegmedewerker")]
        public IActionResult VoegMedewerkerToe(Guid id, [FromBody] string medewerkerEmail)
        {
            if (string.IsNullOrEmpty(medewerkerEmail))
            {
                return BadRequest("E-mailadres is verplicht.");
            }

            try
            {
                _service.VoegMedewerkerToe(id, medewerkerEmail);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Zakelijke huurder niet gevonden.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/ZakelijkeHuurder/{id}/verwijdermedewerker
        [HttpDelete("{id}/verwijdermedewerker")]
        public IActionResult VerwijderMedewerker(Guid id, [FromBody] string medewerkerEmail)
        {
            if (string.IsNullOrEmpty(medewerkerEmail))
            {
                return BadRequest("E-mailadres is verplicht.");
            }

            try
            {
                _service.VerwijderMedewerker(id, medewerkerEmail);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Zakelijke huurder of medewerker niet gevonden.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            return NoContent();
        }
    }
}

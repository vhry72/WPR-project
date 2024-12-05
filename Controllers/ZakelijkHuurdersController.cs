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

        // GET: api/ZakelijkeHuurder
        [HttpGet]
        public ActionResult<IEnumerable<ZakelijkHuurder>> GetAllZakelijkeHuurders()
        {
            var huurders = _service.GetAllZakelijkHuurders();
            return Ok(huurders);
        }

        // GET: api/ZakelijkeHuurder/5
        [HttpGet("{id}")]
        public ActionResult<ZakelijkHuurder> GetZakelijkHuurderById(int id)
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

        // PUT: api/ZakelijkeHuurder/5
        [HttpPut("{id}")]
        public IActionResult UpdateZakelijkeHuurder(int id, [FromBody] ZakelijkHuurder zakelijkHuurder)
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

        // DELETE: api/ZakelijkeHuurder/5
        [HttpDelete("{id}")]
        public IActionResult DeleteZakelijkeHuurder(int id)
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

        // POST: api/ZakelijkeHuurder/5/voegmedewerker
        [HttpPost("{id}/voegmedewerker")]
        public IActionResult VoegMedewerkerToe(int id, [FromBody] string medewerkerEmail)
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

        // DELETE: api/ZakelijkeHuurder/5/verwijdermedewerker
        [HttpDelete("{id}/verwijdermedewerker")]
        public IActionResult VerwijderMedewerker(int id, [FromBody] string medewerkerEmail)
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

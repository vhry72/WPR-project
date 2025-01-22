using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;
using WPR_project.Services;
using WPR_project.Models;
using Microsoft.AspNetCore.Authorization;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticulierHuurderController : ControllerBase
    {
        private readonly ParticulierHuurderService _service;

        public ParticulierHuurderController(ParticulierHuurderService service)
        {
            _service = service;
        }

        // verifieer dat het een particuliere huurder is
        [Authorize(Roles = "ParticuliereHuurder")]
        [HttpGet]
        public ActionResult<IEnumerable<ParticulierHuurderDTO>> GetAll()
        {
            var huurders = _service.GetAll();
            return Ok(huurders);
        }

        // haal huurder op via Id
        [Authorize(Roles = "ParticuliereHuurder")]
        [HttpGet("{id}")]
        public ActionResult<ParticulierHuurderDTO> GetById(Guid id)
        {
            var huurder = _service.GetById(id);
            if (huurder == null)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }

            return Ok(huurder);
        }

        // 2fa authenticatie voor particuliere huurder
        [Authorize(Roles = "ParticulierHuurder")]
        [HttpGet("verify")]
        public IActionResult VerifyEmail(Guid token)
        {
            if (token == Guid.Empty)
            {
                return BadRequest(new { Message = "Verificatietoken is verplicht." });
            }

            var result = _service.VerifyEmail(token);
            if (!result)
            {
                return NotFound(new { Message = "Verificatie mislukt. Token is ongeldig of verlopen." });
            }

            return Ok(new { Message = "E-mail succesvol bevestigd." });
        }

        // update de huurder
        [Authorize(Roles = "ParticuliereHuurder")]
        [HttpPut("{id}")]
        public IActionResult UpdateHuurder(Guid id, [FromBody] ParticulierHuurderDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Console.WriteLine($"ModelState Errors: {string.Join(", ", errors)}");
                return BadRequest(ModelState);
            }

            Console.WriteLine($"Route ID: {id}, DTO ID: {dto.particulierId}");

            if (id != dto.particulierId)
            {
                return BadRequest(new { Message = "ID in de route komt niet overeen met het ID in de body." });
            }

            try
            {
                _service.Update(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }
        }

        // verwijder huurder met gegeven Id
        [HttpDelete("{id}")]
        public IActionResult DeleteHuurder(Guid id)
        {
            try
            {
                _service.Delete(id);
                return Ok(new { Message = "Huurder succesvol verwijderd." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }
        }
        // haal geverifieerde huurders op
        [Authorize(Roles = "ParticulierHuurder")]
        [HttpGet("{id}/isVerified")]
        public IActionResult IsEmailVerified(Guid id)
        {
            var huurder = _service.GetById(id);
            if (huurder == null)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }

            return Ok(new { IsEmailVerified = huurder.IsEmailBevestigd });
        }
    }
}

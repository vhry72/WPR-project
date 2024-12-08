using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;
using WPR_project.Services;
using WPR_project.Models;

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

        /// <summary>
        /// Haalt alle particuliere huurders op.
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<ParticulierHuurderDTO>> GetAll()
        {
            var huurders = _service.GetAll();
            return Ok(huurders);
        }

        /// <summary>
        /// Haalt een specifieke particuliere huurder op via ID.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<ParticulierHuurderDTO> GetById(int id)
        {
            var huurder = _service.GetById(id);
            if (huurder == null)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }

            return Ok(huurder);
        }

        /// <summary>
        /// Registreert een nieuwe particuliere huurder en verstuurt een verificatiemail.
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] ParticulierHuurder pHuurder)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _service.Register(pHuurder);
            return Ok(new { Message = "Registratie succesvol. Controleer je e-mail voor verificatie." });
        }

        /// <summary>
        /// Verifieert de e-mail van een particuliere huurder met een token.
        /// </summary>
        [HttpGet("verify")]
        public IActionResult VerifyEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
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

        /// <summary>
        /// Werkt de gegevens van een bestaande particuliere huurder bij.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateHuurder(int id, [FromBody] ParticulierHuurderDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        /// <summary>
        /// Verwijdert een particuliere huurder op basis van ID.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteHuurder(int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }
        }

        /// <summary>
        /// Controleert of de e-mail van een huurder is geverifieerd.
        /// </summary>
        [HttpGet("{id}/isVerified")]
        public IActionResult IsEmailVerified(int id)
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

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
        public ActionResult<ParticulierHuurderDTO> GetById(Guid id)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Validatiefout",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                _service.Register(pHuurder);
                return Ok(new { Message = "Registratie succesvol. Controleer je e-mail voor verificatie." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Interne serverfout.", Error = ex.Message });
            }
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


        /// <summary>
        /// Verwijdert een particuliere huurder op basis van ID.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteHuurder(Guid id)
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
        public IActionResult IsEmailVerified(Guid id)
        {
            var huurder = _service.GetById(id);
            if (huurder == null)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }

            return Ok(new { IsEmailVerified = huurder.IsEmailBevestigd });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.email) || string.IsNullOrEmpty(loginDto.wachtwoord))
            {
                return BadRequest(new { Message = "E-mail en wachtwoord zijn verplicht." });
            }

            var huurder = _service.GetByEmailAndPassword(loginDto.email, loginDto.wachtwoord);
            if (huurder == null)
            {
                return Unauthorized(new { Message = "Ongeldige e-mail of wachtwoord." });
            }

            return Ok(new
            {
                Id = huurder.particulierId,
                IsEmailVerified = huurder.IsEmailBevestigd,
                Message = "Login succesvol."
            });
        }




    }
}

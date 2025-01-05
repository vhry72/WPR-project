using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;
using WPR_project.Services;
using WPR_project.Models;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedrijfsMedewerkersController : ControllerBase
    {
        private readonly BedrijfsMedewerkersService _service;

        public BedrijfsMedewerkersController(BedrijfsMedewerkersService service)
        {
            _service = service;
        }

        /// <summary>
        /// Haalt alle bedrijfsmedewerkers op.
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<BedrijfsMedewerkersDTO>> GetAll()
        {
            var medewerkers = _service.GetAll();
            return Ok(medewerkers);
        }

        /// <summary>
        /// Haalt een specifieke bedrijfsmedewerker op via ID.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<BedrijfsMedewerkersDTO> GetById(Guid id)
        {
            var medewerker = _service.GetById(id);
            if (medewerker == null)
            {
                return NotFound(new { Message = "Medewerker niet gevonden." });
            }

            return Ok(medewerker);
        }

        /// <summary>
        /// Registreert een nieuwe bedrijfsmedewerker en verstuurt een verificatiemail.
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] BedrijfsMedewerkers medewerker)
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
                _service.Register(medewerker);
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
        /// Werkt de gegevens van een bestaande bedrijfsmedewerker bij.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateMedewerker(Guid id, [FromBody] BedrijfsMedewerkersDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Console.WriteLine($"ModelState Errors: {string.Join(", ", errors)}");
                return BadRequest(ModelState);
            }

            Console.WriteLine($"Route ID: {id}, DTO ID: {dto.bedrijfsMedewerkerId}");

            if (id != dto.bedrijfsMedewerkerId)
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
                return NotFound(new { Message = "Medewerker niet gevonden." });
            }
        }

        /// <summary>
        /// Verwijdert een bedrijfsmedewerker op basis van ID.
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteMedewerker(Guid id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Medewerker niet gevonden." });
            }
        }
    

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { Message = "E-mail en wachtwoord zijn verplicht." });
            }

            var medewerker = _service.GetByEmailAndPassword(loginDto.Email, loginDto.Password);
            if (medewerker == null)
            {
                return Unauthorized(new { Message = "Ongeldige e-mail of wachtwoord." });
            }

            return Ok(new
            {
                Id = medewerker.bedrijfsMedewerkerId,
                Message = "Login succesvol."
            });
        }
    }
}

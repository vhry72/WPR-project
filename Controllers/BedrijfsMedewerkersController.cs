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

        // haal bedrijfsmedewerkers op
        [HttpGet]
        public ActionResult<IEnumerable<BedrijfsMedewerkersDTO>> GetAll()
        {
            var medewerkers = _service.GetAll();
            return Ok(medewerkers);
        }

        // haal medewerker op via Id
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

       // update de medewerker
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

        // verwijder medewerker met gegeven Id
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
    }
}
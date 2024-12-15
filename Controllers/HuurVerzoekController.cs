using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;
using WPR_project.Services;
using WPR_project.Models;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HuurVerzoekController : ControllerBase
    {
        private readonly HuurverzoekService _service;

        public HuurVerzoekController(HuurverzoekService service)
        {
            _service = service;
        }
        
        // Haalt alle huurverzoeken op.
        [HttpGet]
        public IActionResult GetAllHuurVerZoeken()
        {
            try
            {
                var huurverzoeken = _service.GetAllHuurVerzoeken();
                return Ok(huurverzoeken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }
        // Haalt een specifiek Huurverzoek op d.m.v. huurderID
        [HttpGet("{id}")]
        public ActionResult<HuurVerzoek> GetById(Guid id)
        {
            var huurder = _service.GetById(id);
            if (huurder == null)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }

            return Ok(huurder);
        }
        
        // Werkt bij of een HuurVerzoek is GoedGekeurd of Afgekeurd.
        
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] HuurVerzoekDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Console.WriteLine($"ModelState Errors: {string.Join(", ", errors)}");
                return BadRequest(ModelState);
            }

            Console.WriteLine($"Route ID: {id}, DTO ID: {dto.HuurderID}");

            if (id != dto.HuurderID)
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
                return NotFound(new { Message = "Huurverzoek niet gevonden." });
            }
        }
    }
}

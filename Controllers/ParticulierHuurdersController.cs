using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;
using WPR_project.Services;
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

        [Authorize(Roles = "ParticuliereHuurder")]
        [HttpGet("{id}/gegevens")]
        public ActionResult<ParticulierHuurderDTO> GetGegevensById(Guid id)
        {
            var huurder = _service.GetGegevensById(id);
            if (huurder == null)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }

            return Ok(huurder);
        }


        [Authorize(Roles = "ParticuliereHuurder")]
        [HttpPut("{id}")]
        public IActionResult UpdateHuurder(Guid id, [FromBody] ParticulierHuurderWijzigDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Console.WriteLine($"ModelState Errors: {string.Join(", ", errors)}");
                return BadRequest(ModelState);
            }

            try
            {
                _service.Update(id, dto);
                return Ok("de gegevens zijn aangepast");
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }
        }

        [Authorize(Roles = "ParticuliereHuurder")]
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
    }
}

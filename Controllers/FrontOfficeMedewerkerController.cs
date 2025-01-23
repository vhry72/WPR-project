using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;
using WPR_project.Services;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FrontOfficeMedewerkerController : ControllerBase
    {
        private readonly FrontOfficeService _service;

        public FrontOfficeMedewerkerController(FrontOfficeService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllFrontOffice()
        {
            try
            {
                var frontOfficeMedewerkers = _service.GetAll();
                return Ok(frontOfficeMedewerkers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"interne serverfout: {ex.Message}");
            }
        }

        [HttpGet("{id}/gegevens")]
        public ActionResult<FrontofficeMedewerkerWijzigDTO> GetGegevensById(Guid id)
        {
            var huurder = _service.GetGegevensById(id);
            if (huurder == null)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }

            return Ok(huurder);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateHuurder(Guid id, [FromBody] FrontofficeMedewerkerWijzigDTO dto)
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


    }
}

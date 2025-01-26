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
        private readonly HuurverzoekService _huurverzoekService;
        private readonly SchademeldingService _schademeldingService;

        public FrontOfficeMedewerkerController
            (
            FrontOfficeService service, 
            HuurverzoekService huurverzoekService,
            SchademeldingService schademeldingService
            )
        {
            _service = service;
            _huurverzoekService = huurverzoekService;
            _schademeldingService = schademeldingService;

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

        [HttpPost("{HuurverzoekID}/{keuring}/Huurverzoek-isCompleted")]
        public IActionResult HuurverzoekIsCompleted(Guid HuurverzoekID, bool keuring)
        {
            var huurverzoek = _huurverzoekService.GetById(HuurverzoekID);
            if (huurverzoek == null)
            {
                return NotFound(new { Message = "Huurverzoek niet gevonden." });
            }
            try
            {
                _service.HuurverzoekIsCompleted(HuurverzoekID, keuring);
                return Ok("Huurverzoek is afgesloten.");
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("{schademeldingId}/{keuring}/Schademelding-afgehandeld")]
        public IActionResult schademeldingIsCompleted(Guid schademeldingId, bool keuring)
        {
            var huurverzoek = _schademeldingService.GetById(schademeldingId);
            if (huurverzoek == null)
            {
                return NotFound(new { Message = "Huurverzoek niet gevonden." });
            }
            try
            {
                _service.schademeldingIsCompleted(schademeldingId, keuring);
                return Ok("schademelding is afgesloten.");
            }

            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

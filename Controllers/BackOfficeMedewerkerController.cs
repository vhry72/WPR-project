using Microsoft.AspNetCore.Mvc;
using WPR_project.Services;
using WPR_project.DTO_s;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackOfficeMedewerkerController : ControllerBase
    {
        private readonly BackOfficeService _backOfficeService;
        private readonly FrontOfficeService _frontOfficeService;
        

        public BackOfficeMedewerkerController(BackOfficeService backOfficeService, FrontOfficeService frontOfficeService)
        {
            _backOfficeService = backOfficeService;
            _frontOfficeService = frontOfficeService;
        }


        [HttpDelete("verwijdermedewerker/{frontOfficeMedewerkerId}")]
        public IActionResult VerwijderMedewerker(Guid frontOfficeMedewerkerId)
        {
            try
            {
                _frontOfficeService.Delete(frontOfficeMedewerkerId);
                return Ok(new { Message = "Medewerker succesvol verwijderd." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fout: {ex.Message}");
                return StatusCode(500, new { Message = "Er is een interne fout opgetreden.", Details = ex.Message });
            }
        }

        [HttpDelete("VerwijderBackoffice/{Id}")]
        public IActionResult VerwijderBackOfficeMedewerker(Guid id)
        {
            try
            {
                _backOfficeService.Delete(id);
                return Ok(new { Message = "Huurder succesvol verwijderd." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }
        }

        [HttpGet("{id}/gegevens")]
        public ActionResult<BackofficeMedewerkerWijzigDTO> GetGegevensById(Guid id)
        {
            var huurder = _backOfficeService.GetGegevensById(id);
            if (huurder == null)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }

            return Ok(huurder);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateHuurder(Guid id, [FromBody] BackofficeMedewerkerWijzigDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Console.WriteLine($"ModelState Errors: {string.Join(", ", errors)}");
                return BadRequest(ModelState);
            }

            try
            {
                _backOfficeService.Update(id, dto);
                return Ok("de gegevens zijn aangepast");
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }
        }


        [HttpPost("{Abonnementid}/Keuring/{keuring}")]
        public IActionResult keuringAbonnement(Guid Abonnementid, bool keuring)
        {
            try
            {
                _backOfficeService.KeuringAbonnement(Abonnementid, keuring);
                return Ok(new { Message = "Keuring succesvol uitgevoerd." });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fout: {ex.Message}");
                return StatusCode(500, new { Message = "Er is een interne fout opgetreden.", Details = ex.Message });
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.DTO_s;
using WPR_project.Services.Email;
using Microsoft.AspNetCore.Authorization;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WagenparkBeheerderController : ControllerBase
    {
        private readonly WagenparkBeheerderService _service;


        public WagenparkBeheerderController(WagenparkBeheerderService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Wagenparkbeheerder,ZakelijkeHuurder")]
        [HttpGet("{id}/zakelijkeId")]
        public ActionResult<ZakelijkHuurderIdDTO> GetZakelijkeID(Guid id)
        {
            try
            {
                var zakelijkeId = _service.GetZakelijkeId(id);
                return Ok(new ZakelijkHuurderIdDTO { ZakelijkeId = zakelijkeId });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Wagenparkbeheerder,ZakelijkeHuurder")]
        [HttpGet("{id}/AbonnementId")]
        public ActionResult<AbonnementIdDTO> GetAbonnementID(Guid id)
        {
            try
            {
                var AbonnementID = _service.GetAbonnementId(id);
                return Ok(new AbonnementIdDTO { AbonnementId = AbonnementID });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }


        [Authorize(Roles = "Wagenparkbeheerder,ZakelijkeHuurder")]
        [HttpGet("verhuurdevoertuigen/{medewerkerId}")]
        public ActionResult<IEnumerable<Huurverzoek>> GetVerhuurdeVoertuigen(Guid medewerkerId)
        {
            try
            {
                var huurverzoeken = _service.GetVerhuurdeVoertuigen(medewerkerId);
                return Ok(huurverzoeken);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [Authorize(Roles = "Wagenparkbeheerder,ZakelijkeHuurder")]
        [HttpGet("{id}/medewerkers")]
        public IActionResult GetMedewerkersIds(Guid id)
        {
            try
            {
                var medewerkerIds = _service.GetMedewerkersIdsByWagenparkbeheerder(id);

                if (!medewerkerIds.Any())
                    return NotFound(new { Message = "Geen medewerkers gevonden voor de opgegeven WagenparkbeheerderID." });

                return Ok(medewerkerIds);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Er is een onverwachte fout opgetreden.", Details = ex.Message });
            }
        }



        [Authorize(Roles = "ZakelijkeHuurder")]
        [HttpDelete("{id}")]
        public IActionResult DeleteWagenparkbeheerder(Guid id)
        {
            try
            {
                _service.DeleteWagenparkBeheerder(id);
                return Ok(new { Message = "Wagenparkbeheerder succesvol verwijderd." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Wagenparkbeheerder niet gevonden." });
            }
        }


        [Authorize(Roles = "Wagenparkbeheerder,ZakelijkeHuurder")]
        [HttpGet("{id}/medewerker-object")]
        public IActionResult GetMedewerkers(Guid id)
        {
            try
            {
                var medewerkerIds = _service.GetMedewerkersByWagenparkbeheerder(id);

                if (!medewerkerIds.Any())
                    return NotFound(new { Message = "Geen medewerkers gevonden voor de opgegeven WagenparkbeheerderID." });

                return Ok(medewerkerIds);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Er is een onverwachte fout opgetreden.", Details = ex.Message });
            }
        }



        [Authorize(Roles = "Wagenparkbeheerder,ZakelijkeHuurder,Backofficemedewerker")]
        [HttpGet("{id}/gegevens")]
        public ActionResult<WagenparkBeheerderWijzigDTO> GetGegevensById(Guid id)
        {
            var huurder = _service.GetGegevensById(id);
            if (huurder == null)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }

            return Ok(huurder);
        }

        [Authorize(Roles = "ZakelijkeHuurder")]
        [HttpPut("{id}/updateGegevens")]
        public IActionResult UpdateHuurder(Guid id, [FromBody] WagenparkBeheerderWijzigDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                Console.WriteLine($"ModelState Errors: {string.Join(", ", errors)}");
                return BadRequest(ModelState);
            }

            try
            {
                _service.updateWagenparkBeheerderGegevens(id, dto);
                return Ok("de gegevens zijn aangepast");
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }
        }
    }
}
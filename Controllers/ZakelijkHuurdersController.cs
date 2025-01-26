using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;
using WPR_project.Services;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZakelijkeHuurderController : ControllerBase
    {
        private readonly ZakelijkeHuurderService _service;
        private readonly AbonnementService _abonnementService;

        public ZakelijkeHuurderController
            (
            ZakelijkeHuurderService service,
            AbonnementService abonnementService
            )
        {
            _service = service;
            _abonnementService = abonnementService;
        }



        [Authorize(Roles = "Wagenparkbeheerder,ZakelijkeHuurder,Backofficemedewerker")]
        [HttpGet("{id}")]
        public ActionResult<ZakelijkHuurder> GetZakelijkHuurderById(Guid id)
        {
            var huurder = _service.GetZakelijkHuurderById(id);
            if (huurder == null)
            {
                return NotFound("Zakelijke huurder niet gevonden.");
            }
            return Ok(huurder);
        }

        [Authorize(Roles = "Wagenparkbeheerder,ZakelijkeHuurder,Backofficemedewerker")]
        [HttpGet("{id}/AbonnementId")]
        public IActionResult GetAbonnementIdByZakelijkeHuurder(Guid id)
        {
            // Haal het abonnement-ID van de zakelijke huurder op   
            var huurder = _service.GetAbonnementIdByZakelijkeHuurder(id);
            if (huurder == null)
            {
                return NotFound(new { message = "Zakelijke huurder niet gevonden." });
            }

            // Haal de details van het abonnement op
            var abonnement = _abonnementService.GetAbonnementById(huurder.Value);
            if (abonnement == null)
            {
                return NotFound("Abonnement niet gevonden.");
            }
            // return het abonnement 
            return Ok(abonnement);
        }



        [Authorize(Roles = "Wagenparkbeheerder,ZakelijkeHuurder,Backofficemedewerker")]
        [HttpGet("{id}/gegevens")]
        public ActionResult<ZakelijkeHuurderWijzigDTO> GetGegevensById(Guid id)
        {
            var huurder = _service.GetGegevensById(id);
            if (huurder == null)
            {
                return NotFound(new { Message = "Huurder niet gevonden." });
            }

            return Ok(huurder);
        }

        [Authorize(Roles = "ZakelijkeHuurder")]
        [HttpPut("{id}")]
        public IActionResult UpdateHuurder(Guid id, [FromBody] ZakelijkeHuurderWijzigDTO dto)
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

        [Authorize(Roles = "ZakelijkeHuurder")]
        [HttpDelete("{id}")]
        public IActionResult DeleteBedrijf(Guid id)
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

        [Authorize(Roles = "Wagenparkbeheerder,ZakelijkeHuurder")]
        [HttpGet("{id}/WagenparkGegevens")]
        public ActionResult<WagenparkBeheerderGetGegevensDTO> getWagenparkGegevensByZakelijkeId(Guid id)
        {
            var wagenparkBeheerder = _service.GetWagenparkBeheerdersByZakelijkeId(id);

            if (!wagenparkBeheerder.Any())
            {
                return NotFound("Deze zakelijkeId bevat geen wagenparkBeheerders");
            }

            return Ok(wagenparkBeheerder);
        
        }
    }
}

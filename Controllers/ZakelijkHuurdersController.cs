using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Services;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZakelijkeHuurderController : ControllerBase
    {
        private readonly ZakelijkeHuurderService _service;
        private readonly UserManagerService _userManager;
        private readonly AbonnementService _abonnementService;

        public ZakelijkeHuurderController
            (
            ZakelijkeHuurderService service,
            UserManagerService userManager,
            AbonnementService abonnementService
            )
        {
            _service = service;
            _userManager = userManager;
            _abonnementService = abonnementService;
        }


       
        [HttpGet("verify")]
        public IActionResult VerifyEmail(Guid token)
        {
            if(token == Guid.Empty)
            {
                return BadRequest(new { Message = "Verificatietoken is verplicht." });
            }

            var result = _service.VerifyEmail(token);
            if (!result)
            {
                return NotFound("Verificatie mislukt. Token is ongeldig of verlopen.");
            }

            return Ok("Je e-mail is succesvol bevestigd. Je kunt nu inloggen.");
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<ZakelijkHuurder>> GetAllZakelijkeHuurders()
        {
            var huurders = _service.GetAllZakelijkHuurders();
            return Ok(huurders);
        }

       
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




        [HttpPost]
        public ActionResult AddZakelijkeHuurder([FromBody] ZakelijkHuurder zakelijkHuurder)
        {
            if (zakelijkHuurder == null)
            {
                return BadRequest("Ongeldige gegevens voor huurder.");
            }

            _service.AddZakelijkeHuurder(zakelijkHuurder);
            return CreatedAtAction(nameof(GetZakelijkHuurderById), new { id = zakelijkHuurder.zakelijkeId }, zakelijkHuurder);
        }


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


        [Authorize]
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


        [HttpPost("{id}/voegmedewerker")]
        public IActionResult VoegMedewerkerToe(Guid id, [FromBody] BedrijfsMedewerkers medewerker)
        {
            if (string.IsNullOrEmpty(medewerker.medewerkerEmail))
            {
                return BadRequest("E-mailadres is verplicht.");
            }

            try
            {
                _service.VoegMedewerkerToe(id, medewerker.medewerkerNaam, medewerker.medewerkerEmail);
                return Ok(new { Message = "Medewerker succesvol toegevoegd." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Zakelijke huurder niet gevonden.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

      
        [HttpDelete("{id}/verwijdermedewerker")]
        public IActionResult VerwijderMedewerker(Guid id, [FromBody] string medewerkerEmail)
        {
            if (string.IsNullOrEmpty(medewerkerEmail))
            {
                return BadRequest("E-mailadres is verplicht.");
            }

            try
            {
                _service.VerwijderMedewerker(id, medewerkerEmail);
                return Ok(new { Message = "Medewerker succesvol verwijderd." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Zakelijke huurder of medewerker niet gevonden.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }


        [HttpPost("{zakelijkeHuurderId}/wagenparkbeheerder")]
        public IActionResult AddWagenparkBeheerder(Guid zakelijkeHuurderId, [FromBody] WagenparkBeheerder beheerder)
        {
            try
            {
                var zakelijkeHuurder = _service.GetZakelijkHuurderById(zakelijkeHuurderId);
                if (zakelijkeHuurder == null)
                {
                    return BadRequest("Zakelijke huurder niet gevonden. Een wagenparkbeheerder kan alleen worden toegevoegd aan een bestaande zakelijke huurder.");
                }

                if (!beheerder.bedrijfsEmail.EndsWith($"@{zakelijkeHuurder.domein}"))
                {
                    return BadRequest("E-mailadres van de wagenparkbeheerder moet overeenkomen met het domein van de zakelijke huurder.");
                }

                _service.AddWagenparkBeheerder(zakelijkeHuurderId, beheerder);
                return CreatedAtAction(nameof(GetZakelijkHuurderById), new { id = beheerder.beheerderId }, beheerder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }
    }
}

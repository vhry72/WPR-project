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
            var huurder = _service.GetAbonnementIdByZakelijkeHuurder(id);
            var abonnement = _abonnementService.GetAbonnementById(huurder);
            if (huurder == null)
            {
                return NotFound("Zakelijke huurder niet gevonden.");
            }
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

       
        [HttpPut("{id}")]
        public IActionResult UpdateZakelijkeHuurder(Guid id, [FromBody] ZakelijkHuurder zakelijkHuurder)
        {
            if (zakelijkHuurder == null || zakelijkHuurder.zakelijkeId != id)
            {
                return BadRequest("ID komt niet overeen met de gegevens.");
            }

            try
            {
                _service.Update(id, zakelijkHuurder);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Zakelijke huurder niet gevonden.");
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public IActionResult DeleteZakelijkeHuurder(Guid id)
        {
            try
            {
                // Haal de zakelijke huurder op
                var zakelijkeHuurder = _service.GetZakelijkHuurderById(id);
                if (zakelijkeHuurder == null)
                {
                    return NotFound("Zakelijke huurder niet gevonden.");
                }

                // Haal het AspNetUsersId op
                var aspNetUserId = zakelijkeHuurder.AspNetUserId;
                if(aspNetUserId == null)
                {
                    return NotFound("Asp User niet gevonden");
                }


                // Verwijder de zakelijke huurder
                _service.Delete(id);

                // Zoek de gebruiker in AspNetUsers
                var user = _userManager.FindByIdAsync(aspNetUserId).Result;
                if (user == null)
                {
                    return NotFound("De gekoppelde gebruiker in AspNetUsers is niet gevonden.");
                }

                // Verwijder de gebruiker uit AspNetUsers
                var result = _userManager.DeleteAsync(aspNetUserId).Result;
                if (!result)
                {
                    return StatusCode(500, "Er is een fout opgetreden bij het verwijderen van de gebruiker.");
                }
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Zakelijke huurder niet gevonden.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Er is een fout opgetreden: {ex.Message}");
            }

            return NoContent();
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

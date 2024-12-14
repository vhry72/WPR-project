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

        public ZakelijkeHuurderController(ZakelijkeHuurderService service)
        {
            _service = service;
        }

        [HttpPost("registerDTO")]
        public ActionResult RegisterZakelijkeHuurder([FromBody] ZakelijkeHuurderDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.bedrijfsEmail))
            {
                return BadRequest("Ongeldige gegevens voor registratie.");
            }

            var zakelijkHuurder = new ZakelijkHuurder
            {
                zakelijkeId = Guid.NewGuid(),
                adres = dto.adres,
                KVKNummer = dto.KVKNummer,
                bedrijsEmail = dto.bedrijfsEmail,
                EmailBevestigingToken = Guid.NewGuid().ToString(),
                IsEmailBevestigd = false,
                telNummer = dto.telNummer,
                bedrijfsNaam = dto.bedrijfsNaam,
                wachtwoord = dto.wachtwoord
            };

            _service.RegisterZakelijkeHuurder(zakelijkHuurder);
            return Ok("Registratie succesvol. Controleer je e-mail voor de verificatielink.");
        }


        /// <summary>
        /// Verifieert een e-mailadres
        /// </summary>
        [HttpGet("verify")]
        public IActionResult VerifyEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Verificatie token is verplicht.");
            }

            var result = _service.VerifyEmail(token);
            if (!result)
            {
                return NotFound("Verificatie mislukt. Token is ongeldig of verlopen.");
            }

            return Ok("Je e-mail is succesvol bevestigd. Je kunt nu inloggen.");
        }

        /// <summary>
        /// Haalt alle zakelijke huurders op
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<ZakelijkHuurder>> GetAllZakelijkeHuurders()
        {
            var huurders = _service.GetAllZakelijkHuurders();
            return Ok(huurders);
        }

        /// <summary>
        /// Haalt een specifieke zakelijke huurder op via ID
        /// </summary>
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

        /// <summary>
        /// Voegt een nieuwe zakelijke huurder toe
        /// </summary>
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

        /// <summary>
        /// Wijzigt een bestaande zakelijke huurder
        /// </summary>
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

        /// <summary>
        /// Verwijdert een zakelijke huurder via ID
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteZakelijkeHuurder(Guid id)
        {
            try
            {
                _service.Delete(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Zakelijke huurder niet gevonden.");
            }

            return NoContent();
        }

        /// <summary>
        /// Voegt een medewerker toe aan een zakelijke huurder
        /// </summary>
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

        /// <summary>
        /// Verwijdert een medewerker van een zakelijke huurder
        /// </summary>
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

        /// <summary>
        /// Voegt een wagenparkbeheerder toe
        /// </summary>
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

using Microsoft.AspNetCore.Mvc;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.DTO_s;
using WPR_project.Services.Email;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WagenparkBeheerderController : ControllerBase
    {
        private readonly WagenparkBeheerderService _service;
        private readonly IEmailService _emailService;

        public WagenparkBeheerderController(WagenparkBeheerderService service, IEmailService emailService)
        {
            _service = service;
            _emailService = emailService;
        }

        /// <summary>
        /// Haalt alle wagenparkbeheerders op
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<WagenparkBeheerder>> GetAllBeheerders()
        {
            var beheerders = _service.GetWagenparkBeheerders();
            return Ok(beheerders);
        }

        /// <summary>
        /// Haalt een wagenparkbeheerder op via ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<WagenparkBeheerder> GetBeheerderById(Guid id)
        {
            var beheerder = _service.GetBeheerderById(id);
            if (beheerder == null)
            {
                return NotFound(new { Message = "Beheerder niet gevonden." });
            }
            return Ok(beheerder);
        }

        /// <summary>
        /// Voegt een nieuwe wagenparkbeheerder toe
        /// </summary>
        [HttpPost]
        public ActionResult AddBeheerder([FromBody] WagenparkBeheerderDTO beheerderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Ongeldige invoer.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var beheerder = new WagenparkBeheerder
            {
                beheerderId = Guid.NewGuid(),
                beheerderNaam = beheerderDTO.beheerderNaam,
                bedrijfsEmail = beheerderDTO.beheerderEmail,
                wachtwoord = beheerderDTO.wachtwoord
            };

            _service.AddWagenparkBeheerder(beheerder);
            return CreatedAtAction(nameof(GetBeheerderById), new { id = beheerder.beheerderId }, beheerder);
        }

        /// <summary>
        /// Wijzigt een bestaande wagenparkbeheerder
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult UpdateBeheerder(Guid id, [FromBody] WagenparkBeheerder beheerder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _service.UpdateWagenparkBeheerder(id, beheerder);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Verwijdert een wagenparkbeheerder via ID
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteBeheerder(Guid id)
        {
            try
            {
                _service.DeleteWagenparkBeheerder(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }


        // voeg een medewerker aan een zakelijke Huurder toe
        [HttpPost("{zakelijkeId}/voegmedewerker")]
        public IActionResult VoegMedewerkerToe(Guid zakelijkeId, [FromBody] BedrijfsMedewerkers medewerker)
        {
            if (medewerker == null)
            {
                return BadRequest(new { Message = "De medewerkergegevens zijn vereist." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Ongeldige invoer.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                // aanroep om medewerker toe te voegen
                _service.VoegMedewerkerToe(zakelijkeId, medewerker.medewerkerNaam, medewerker.medewerkerEmail, medewerker.wachtwoord);

                // Verstuur bevestigingsmail naar de medewerker
                string emailBody = $@"
            <h2>Welkom bij ons systeem, {medewerker.medewerkerNaam}</h2>
            <p>U bent succesvol toegevoegd aan het bedrijfsabonnement.</p>
            <p>Uw inloggegevens zijn:</p>
            <ul>
                <li><strong>E-mail:</strong> {medewerker.medewerkerEmail}</li>
                <li><strong>Wachtwoord:</strong> {medewerker.wachtwoord}</li>
            </ul>
            <p>Gebruik deze gegevens om in te loggen op ons systeem.</p>
            <br/>
            <p>Met vriendelijke groet,</p>
            <p>Het supportteam</p>";

                _emailService.SendEmail(
                    medewerker.medewerkerEmail,
                    "Welkom bij het systeem - Bevestigingsmail",
                    emailBody
                );

                return Ok(new
                {
                    Message = "Medewerker succesvol toegevoegd en bevestigingsmail verzonden.",
                    Medewerker = new
                    {
                        medewerker.medewerkerNaam,
                        medewerker.medewerkerEmail
                    }
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fout: {ex.Message}");
                return StatusCode(500, new { Message = "Er is een interne fout opgetreden.", Details = ex.Message });
            }
        }

        //test endpoint
        [HttpPost("test")]
        public IActionResult Test([FromBody] object body)
        {
            return Ok(body);
        }


        /// <summary>
        /// Verwijdert een medewerker van een zakelijke huurder
        /// </summary>
        [HttpDelete("{zakelijkeId}/verwijdermedewerker/{medewerkerId}")]
        public IActionResult VerwijderMedewerker(Guid zakelijkeId, Guid medewerkerId)
        {
            try
            {
                var medewerker = _service.GetMedewerkerById(medewerkerId);
                if (medewerker == null)
                {
                    return NotFound(new { Message = "Medewerker niet gevonden." });
                }

                _service.VerwijderMedewerker(zakelijkeId, medewerkerId);

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
    }
}

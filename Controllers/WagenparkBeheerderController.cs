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

        // haal de wagenparkbeheerders op
        [HttpGet]
        public ActionResult<IEnumerable<WagenparkBeheerder>> GetAllBeheerders()
        {
            var beheerders = _service.GetWagenparkBeheerders();
            return Ok(beheerders);
        }

        // haal wagenparkbeheerders op via Id
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
        // haal wagenparkbeheerders op via bedrijfsemail
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

        // haal wagenparkbeheerders op via abonnementId
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
        // haal wagenparkbeheerders op van verhuurde voertuigen voor medewerkers
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

        // haal de medewerkers bij een wagenparkbeheerder op
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

        // maak wagenparkbeheerder aan
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
                bedrijfsEmail = beheerderDTO.bedrijfsEmail,
                wachtwoord = beheerderDTO.wachtwoord
            };

            _service.AddWagenparkBeheerder(beheerder);
            return CreatedAtAction(nameof(GetBeheerderById), new { id = beheerder.beheerderId }, beheerder);
        }


        // update een bestaande wagenparkbeheerder
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

        // verwijder een wagenparkbeheerder
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

        // haalt de medewerkergegevens op die toegevoegd zijn

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


        // wijzig de bedrijfsmedewerker
        [HttpPut("{id}/medewerker-wijzigen")]
        public IActionResult GetMedewerkersWijzigen(Guid id)
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

            [HttpPost("/zet-limiet")]
             IActionResult ZetVoertuigenLimiet([FromQuery] Guid beheerderId, [FromQuery] int nieuwLimiet)
            {
                try
                {
                    _service.ZetVoertuigenLimiet(beheerderId, nieuwLimiet);
                    return Ok("Limiet succesvol ingesteld.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpPut("/verhoog-limiet")]
             IActionResult VerhoogVoertuigenLimiet([FromQuery] Guid beheerderId, [FromQuery] int verhoging)
            {
                try
                {
                    _service.VerhoogVoertuigenLimiet(beheerderId, verhoging);
                    return Ok("Limiet succesvol verhoogd.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpPut("/verlaag-limiet")]
             IActionResult VerlaagVoertuigenLimiet([FromQuery] Guid beheerderId, [FromQuery] int verlaging)
            {
                try
                {
                    _service.VerlaagVoertuigenLimiet(beheerderId, verlaging);
                    return Ok("Limiet succesvol verlaagd.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

           
        // verwijderen van een medewerker van een wagenparkbeheerder
        [HttpDelete("{zakelijkeId}/verwijdermedewerker/{medewerkerId}")]
        public IActionResult VerwijderMedewerker(Guid zakelijkeId, Guid medewerkerId)
        {
            try
            {
                var medewerker = _service.GetMedewerkerById(medewerkerId);
                if (medewerker == null)
                {
                    // Gebruik expliciet NotFoundObjectResult
                    return new NotFoundObjectResult(new { Message = "Medewerker niet gevonden." });
                }

                _service.VerwijderMedewerker(zakelijkeId, medewerkerId);
                return new OkObjectResult(new { Message = "Medewerker succesvol verwijderd." });
            }
            catch (KeyNotFoundException ex)
            {
                
                return new NotFoundObjectResult(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fout: {ex.Message}");
                // geef statuscode 500 terug met een objectresult
                return new ObjectResult(new { Message = "Er is een interne fout opgetreden.", Details = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }
    }
}
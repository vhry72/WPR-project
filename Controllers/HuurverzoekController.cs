using Microsoft.AspNetCore.Mvc;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.Services.Email;

[Route("api/[controller]")]
[ApiController]
public class HuurverzoekController : ControllerBase
{
    private readonly HuurverzoekService _service;
    private readonly IEmailService _emailService;
    private readonly VoertuigService _voertuigService;

    public HuurverzoekController(HuurverzoekService service, IEmailService emailService, VoertuigService voertuigService)
    {
        _service = service;
        _emailService = emailService;
        _voertuigService = voertuigService;
    }

    // zie de actieve huurverzoeken van een huurder
    [HttpGet("check-active/{huurderId}")]
    public IActionResult CheckActiveHuurverzoek(Guid huurderId)
    {
        var hasActiveRequest = _service.HasActiveHuurverzoek(huurderId);
        return Ok(new { hasActiveRequest });
    }

    // zie de beantwoorde huurverzoeken van een huurder
    [HttpGet("check-Beantwoorde/{huurderId}")]
    public IActionResult CheckBeantwoordeHuurverzoek(Guid huurderId)
    {
        var hasActiveRequest = _service.HasAnsweredHuurverzoek(huurderId);
        return Ok(new { hasActiveRequest });
    }

    // Haal alle beschikbare voertuigen op
    [HttpGet("BeschikbareVoertuigen/{startDatum}/{eindDatum}")]
    public IActionResult GetAvailableVehicles(DateTime startDatum, DateTime eindDatum)
    {
        try
        {
            var availableVehicles = _service.GetAvailableVehicles(startDatum, eindDatum);
            return Ok(availableVehicles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }

    // Maak een huurverzoek aan
    [HttpPost]
    public IActionResult CreateHuurVerzoek([FromBody] HuurVerzoekDTO huurVerzoekDto)
    {
        if (huurVerzoekDto == null || huurVerzoekDto.VoertuigId == Guid.Empty)
        {
            return BadRequest("Voertuig-ID is verplicht.");
        }

        var voertuig = _voertuigService.GetById(huurVerzoekDto.VoertuigId);
        if (voertuig == null)
        {
            return NotFound("Voertuig niet gevonden.");
        }

        var huurVerzoek = new Huurverzoek
        {
            HuurVerzoekId = Guid.NewGuid(),
            HuurderID = huurVerzoekDto.HuurderID,
            VoertuigId = voertuig.voertuigId,
            beginDate = huurVerzoekDto.beginDate,
            endDate = huurVerzoekDto.endDate,
            approved = false,
            isBevestigd = false
        };

        _service.Add(huurVerzoek);

        // Haal e-mailadres op op basis van huurderID
        var email = _service.GetEmailByHuurderId(huurVerzoek.HuurderID);
        if (string.IsNullOrWhiteSpace(email))
        {
            return NotFound("E-mailadres voor de huurder kon niet worden gevonden.");
        }

        // Verstuur een bevestigingsmail
        var subject = "Bevestiging van uw huurverzoek";
        var body = $@"Beste gebruiker,<br/><br/>
                  Uw huurverzoek is succesvol geregistreerd.<br/>
                  Startdatum: {huurVerzoek.beginDate:dd-MM-yyyy}<br/>
                  Einddatum: {huurVerzoek.endDate:dd-MM-yyyy}<br/><br/>
                  Met vriendelijke groet,<br/>Het Team";

        _emailService.SendEmail(email, subject, body);

        return Ok(new { Message = "Huurverzoek succesvol ingediend. Een bevestiging is verzonden naar uw e-mailadres." });
    }


    // Haal alle huurverzoeken op van een bepaalde Id
    [HttpGet("GetByHuurderID/{id}")]
    public IActionResult GetHuurverzoekenByHuurderID(Guid id)
    {
        try
        {
            var Huurverzoeken = _service.GetHuurverzoekByHuurderID(id);
            return Ok(Huurverzoeken);
        }
        catch (Exception ex) 
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }

    // haal alle verzoeken op
    [HttpGet]
    public IActionResult GetAllHuurVerZoeken()
    {
        try
        {
            var huurverzoeken = _service.GetAllHuurVerzoeken();
            return Ok(huurverzoeken);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }

    // haalde actieve verzoeken op
    [HttpGet("GetAllActive")]
    public IActionResult GetAllActiveHuurVerZoeken()
    {
        try
        {
            var huurverzoeken = _service.GetAllActiveHuurVerzoeken();
            return Ok(huurverzoeken);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }

    // haal alle beantwoorde verzoeken op
    [HttpGet("GetAllBeantwoorde")]
    public IActionResult GetAllBeantwoordeVerZoeken()
    {
        try
        {
            var huurverzoeken = _service.GetAllBeantwoordeHuurVerzoeken();
            return Ok(huurverzoeken);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }

    // haal alle afgekeurde verzoeken op
    [HttpGet("GetAllAfgekeurde")]
    public IActionResult GetAllAfgekeurde()
    {
        try
        {
            var huurverzoeken = _service.GetAllAfgekeurde();
            return Ok(huurverzoeken);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }

    // haal alle goedgekeurde verzoeken op
    [HttpGet("GetAllGoedGekeurde")]
    public IActionResult GetAllGoedGekeurde()
    {
        try
        {
            var huurverzoeken = _service.GetAllGoedGekeurde();
            return Ok(huurverzoeken);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }

    // Haal een specifieke huurverzoek op d.m.v. huurderID
    [HttpGet("{id}")]
    public ActionResult<Huurverzoek> GetById(Guid id)
    {
        var huurder = _service.GetById(id);
        if (huurder == null)
        {
            return NotFound(new { Message = "Huurder niet gevonden." });
        }
        return Ok(huurder);
    }

    // Werkt bij of een HuurVerzoek is GoedGekeurd of Afgekeurd en of die bevestigd is.
    [HttpPut("KeurGoed/{id}")]
    public IActionResult Update(Guid id, [FromBody] HuurVerzoekDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            Console.WriteLine($"ModelState Errors: {string.Join(", ", errors)}");
            return BadRequest(ModelState);
        }
        Console.WriteLine($"Route ID: {id}, DTO ID: {dto.HuurderID}");
        if (id != dto.HuurderID)
        {
            return BadRequest(new { Message = "ID in de route komt niet overeen met het ID in de body." });
        }
        try
        {
            _service.Update(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = "Huurverzoek niet gevonden." });
        }
    }

    // update status van huurverzoek
    [HttpPut("keuring/{id}/{approved}")]
    public IActionResult WeigerRequest(Guid id, bool approved)
    {
        try
        {
            var huurverzoek = _service.GetById(id);
            if (huurverzoek == null)
            {
                return NotFound(new { Message = "Huurverzoek niet gevonden." });
            }

            // Update de goedkeuring en bevestiging
            huurverzoek.approved = approved;
            huurverzoek.isBevestigd = true;

            _service.Update(id, huurverzoek);

            // Haal e-mail op op basis van huurderID
            var email = _service.GetEmailByHuurderId(huurverzoek.HuurderID);

            string subject = "Bevestiging van uw huurverzoek";
            string body = approved
                ? $"Beste gebruiker,<br/><br/>Uw huurverzoek is goedgekeurd.<br/>" +
                  $"Startdatum: {huurverzoek.beginDate}<br/>Einddatum: {huurverzoek.endDate}<br/><br/>" +
                  "Met vriendelijke groet,<br/>Het Team"
                : $"Beste gebruiker,<br/><br/>Uw huurverzoek is afgekeurd.<br/><br/>" +
                  "Met vriendelijke groet,<br/>Het Team";

            _emailService.SendEmail(email, subject, body);

            return approved
                ? Ok(new { Message = "Huurverzoek goedgekeurd." })
                : Ok(new { Message = "Huurverzoek afgekeurd." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }

}





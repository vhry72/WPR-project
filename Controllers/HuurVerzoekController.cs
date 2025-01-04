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

    public HuurverzoekController(HuurverzoekService service, IEmailService emailService)
    {
        _service = service;
        _emailService = emailService;
    }

    [HttpGet("check-active/{huurderId}")]
    public IActionResult CheckActiveHuurverzoek(Guid huurderId)
    {
        var hasActiveRequest = _service.HasActiveHuurverzoek(huurderId);
        return Ok(new { hasActiveRequest });
    }

    [HttpPost("verzoek")]
    public IActionResult Verzoek([FromBody] Huurverzoek huurverzoek)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                Message = "Validatiefout",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        try
        {
            // Verwerk het huurverzoek
            _service.Add(huurverzoek);

            // Haal e-mail op op basis van huurderID
            var email = _service.GetEmailByHuurderId(huurverzoek.HuurderID);

            // Verstuur een bevestigingsmail
            var subject = "Bevestiging van uw huurverzoek";
            var body = $"Beste gebruiker,<br/><br/>Uw huurverzoek is succesvol geregistreerd.<br/>" +
                       $"Startdatum: {huurverzoek.beginDate}<br/>Einddatum: {huurverzoek.endDate}<br/><br/>" +
                       "Met vriendelijke groet,<br/>Het Team";

            _emailService.SendEmail(email, subject, body);

            return Ok(new { Message = "Huurverzoek succesvol ingediend. Wij nemen zo snel mogelijk contact op." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Interne serverfout.", Error = ex.Message });
        }
    }


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

    // Haalt een specifiek Huurverzoek op d.m.v. huurderID
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

    [HttpPut("approve/{id}/{approved}")]
    public IActionResult ApproveRequest(Guid id, bool approved, bool isBevestigd)
    {
        try
        {
            var huurverzoek = _service.GetById(id);
            if (huurverzoek == null)
            {
                return NotFound(new { Message = "Huurverzoek niet gevonden." });
            }

            huurverzoek.approved = true; // Update de goedkeuring
            huurverzoek.isBevestigd = true; //update de bevestiging
            huurverzoek.Voertuig.voertuigBeschikbaar = false;
            _service.Update(id, huurverzoek); // Pas de update correct toe
            return Ok(new { Message = "Huurverzoek goedgekeurd." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }
    [HttpPut("weiger/{id}/{approved}")]
    public IActionResult WeigerRequest(Guid id, bool approved, bool isBevestigd)
    {
        try
        {
            var huurverzoek = _service.GetById(id);
            if (huurverzoek == null)
            {
                return NotFound(new { Message = "Huurverzoek niet gevonden." });
            }

            huurverzoek.approved = false; // Update de goedkeuring
            huurverzoek.isBevestigd = true; //update de bevestiging
            
            _service.Update(id, huurverzoek); // Pas de update correct toe
            return Ok(new { Message = "Huurverzoek Afgekeurd." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }


}




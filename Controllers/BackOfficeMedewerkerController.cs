using Microsoft.AspNetCore.Mvc;
using System;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.DTO_s;
using Microsoft.EntityFrameworkCore;
using WPR_project.Services.Email;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackOfficeMedewerkerController : ControllerBase
    {
        private readonly BackOfficeService _backOfficeService;
        private readonly FrontOfficeService _frontOfficeService;
        private readonly IEmailService _emailService;

        public BackOfficeMedewerkerController(BackOfficeService backOfficeService, FrontOfficeService frontOfficeService, IEmailService emailService)
        {
            _backOfficeService = backOfficeService;
            _frontOfficeService = frontOfficeService;
            _emailService = emailService;
        }

        // frontoffice aanmaken
        [HttpPost("voegFrontOffice")]
        public IActionResult VoegFrontOfficeMedewerkerToe([FromBody] FrontofficeMedewerkerDTO medewerker)
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
                // Aanroep om medewerker toe te voegen met de DTO data
                _frontOfficeService.VoegMedewerkerToe(medewerker.medewerkerId, medewerker.medewerkerNaam, medewerker.medewerkerEmail, medewerker.wachtwoord);

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
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fout: {ex.Message}");
                return StatusCode(500, new { Message = "Er is een interne fout opgetreden.", Details = ex.Message });
            }
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



    }
}

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

        [HttpDelete("verwijdermedewerker/{frontOfficeMedewerkerId}/{medewerkerId}")]
        public IActionResult VerwijderMedewerker(Guid frontOfficeMedewerkerId, Guid medewerkerId)
        {
            try
            {
                _frontOfficeService.VerwijderMedewerker(frontOfficeMedewerkerId, medewerkerId);
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

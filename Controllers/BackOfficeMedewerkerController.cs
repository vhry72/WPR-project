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
        private readonly BackOfficeService _service;
        private readonly IEmailService _emailService;


        public BackOfficeMedewerkerController(BackOfficeService service, IEmailService emailService)
        {
            _service = service;
            _emailService = emailService;
        }
        // voeg een medewerker aan een zakelijke Huurder toe
        [HttpPost("{FrontofficeMedewerkerId}/voegFrontOffice")]
        public IActionResult VoegFrontOfficeMedewerkerToe(Guid FrontOfficeMedewerkerId, [FromBody] FrontofficeMedewerkerDTO medewerker)
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
                _service.VoegMedewerkerToe(FrontOfficeMedewerkerId, medewerker.medewerkerNaam, medewerker.medewerkerEmail, medewerker.wachtwoord);

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
        [HttpDelete("{FrontOfficeMedewerkerId}/verwijdermedewerker/{medewerkerId}")]
        public IActionResult VerwijderMedewerker(Guid frontOfficeMedewerkerId, Guid medewerkerId)
        {
            try
            {
                var medewerker = _service.GetMedewerkerById(medewerkerId);
                if (medewerker == null)
                {
                    // Gebruik expliciet NotFoundObjectResult
                    return new NotFoundObjectResult(new { Message = "Medewerker niet gevonden." });
                }

                _service.VerwijderMedewerker(frontOfficeMedewerkerId, medewerkerId);

                // Gebruik expliciet OkObjectResult
                return new OkObjectResult(new { Message = "Medewerker succesvol verwijderd." });
            }
            catch (KeyNotFoundException ex)
            {
                // Gebruik expliciet NotFoundObjectResult
                return new NotFoundObjectResult(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fout: {ex.Message}");
                // Gebruik expliciet ObjectResult met StatusCode 500
                return new ObjectResult(new { Message = "Er is een interne fout opgetreden.", Details = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }
    }

}

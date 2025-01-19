using Microsoft.AspNetCore.Mvc;
using WPR_project.Services;
using WPR_project.DTO_s;


namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrivacyVerklaringController : ControllerBase
    {
        private readonly PrivacyVerklaringService _privacyVerklaringService;

        public PrivacyVerklaringController(PrivacyVerklaringService privacyVerklaringService)
        {
            _privacyVerklaringService = privacyVerklaringService;
        }

        [HttpGet("Laatste-variant-privacyVerklaring")]
        public IActionResult GetLatestPrivacyVerklaring()
        {
            var privacyVerklaring = _privacyVerklaringService.GetLatestPrivacyVerklaring();
            return Ok(privacyVerklaring);
        }

        [HttpGet("AllePrivacyVerklaringen")]
        public IActionResult GetAllPrivacyVerklaringen()
        {
            var privacyVerklaringen = _privacyVerklaringService.GetAllPrivacyVerklaringen();
            return Ok(privacyVerklaringen);
        }

        [HttpPost("Voeg-privacyVerklaring-toe")]
        public IActionResult AddPrivacyVerklaring([FromBody] PrivacyVerklaringDTO privacyVerklaring)
        {
            _privacyVerklaringService.Add(privacyVerklaring);
            //_privacyVerklaringService.SendEmailToBackOffice(privacyVerklaring.MedewerkerId);
            return Ok();
        }

    }
}

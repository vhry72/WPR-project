using Microsoft.AspNetCore.Mvc;
using WPR_project.Services;
using WPR_project.DTO_s;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoertuigController : ControllerBase
    {
        private readonly VoertuigService _voertuigService;

        public VoertuigController(VoertuigService voertuigService)
        {
            _voertuigService = voertuigService;
        }




        [HttpGet("VoertuigType")]
        public IActionResult GetVoertuigTypeVoertuigen([FromQuery] string voertuigType)
        {
            try
            {
                var voertuigen = _voertuigService.GetVoertuigTypeVoertuigen(voertuigType);
                return Ok(voertuigen);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetVoertuigDetails(Guid id)
        {
            try
            {
                var voertuig = _voertuigService.GetVoertuigDetails(id);
                return Ok(voertuig);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }



        [Authorize(Roles = "Backofficemedewerker")]
        [HttpPut("veranderGegevens/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> veranderGegevens(Guid id, [FromForm] VoertuigWijzigingDTO DTO, [FromForm] IFormFile afbeelding)
        {
            if (afbeelding != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await afbeelding.CopyToAsync(memoryStream);
                    DTO.Afbeelding = memoryStream.ToArray();
                }
            }

            try
            {
                _voertuigService.veranderGegevens(id, DTO);
                return Ok(new { Message = "Voertuiggegevens succesvol bijgewerkt." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }



        [Authorize(Roles = "Backofficemedewerker,Frontofficemedewerker")]
        [HttpPut("veranderBeschikbaar/{id}/{voertuigBeschikbaar}")]
        public IActionResult neemIn(Guid id, bool voertuigBeschikbaar)
        {
            try
            {
                var voertuig = _voertuigService.GetById(id);
                if (voertuig == null)
                {
                    return NotFound(new { Message = "Voertuig niet gevonden." });
                }
                voertuig.voertuigBeschikbaar = voertuigBeschikbaar;// Update de goedkeuring               
                _voertuigService.UpdateVoertuig(id, voertuig); // Pas de update correct toe
                return Ok(new { Message = "Voertuigingenomen." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }

        [Authorize(Roles = "Backofficemedewerker,Frontofficemedewerker")]
        [HttpPut("maaknotitie/{id}")]
        public IActionResult maakNotitie(Guid id, [FromBody] VoertuigDTO voertuigNotitieDTO)
        {
            try
            {
                var voertuig = _voertuigService.GetById(id);
                if (voertuig == null)
                {
                    return NotFound(new { Message = "Voertuig niet gevonden." });
                }
                voertuig.notitie = voertuigNotitieDTO.notitie;
                _voertuigService.UpdateVoertuig(id, voertuig);
                return Ok(new { Message = "Notitie toegevoegd." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }

        [Authorize(Roles = "Backofficemedewerker")]
        [HttpDelete("verwijderVoertuig/{id}")]
        public IActionResult DeleteVoertuig(Guid id)
        {
            try
            {
                _voertuigService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Voertuig niet gevonden." });
            }
        }


        [Authorize(Roles = "Backofficemedewerker")]
        [HttpPost("maakVoertuig")]
        public IActionResult maakVoertuig([FromBody] VoertuigDTO voertuig)
        {
            if (voertuig == null)
            {
                return BadRequest(new { Message = "Invalid data" });
            }

            try
            {
                _voertuigService.newVoertuig(voertuig);
                return Ok(new { Message = "Voertuig succesvol aangemaakt" });
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., to a file or monitoring tool)
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Details = ex.InnerException?.Message ?? ex.Message
                });
            }

        }
    }
}

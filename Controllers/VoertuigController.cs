using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.DTO_s;
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

        [HttpGet("filter")]
        public IActionResult GetFilteredVoertuigen([FromQuery] string voertuigType, DateTime? startDatum, DateTime? eindDatum, [FromQuery] string sorteerOptie)
        {
            try
            {
                var voertuigen = _voertuigService.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, sorteerOptie);
                return Ok(voertuigen);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAllVoertuigen()
        {
            try
            {
                var voertuigen = _voertuigService.GetAllVoertuigen();
                return Ok(voertuigen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
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
        [HttpGet("getByKenteken/{kenteken}")]
        public IActionResult GetVoertuigbyKenteken(string kenteken)
        {
            try
            {
                var voertuig = _voertuigService.GetVoertuigByKenteken(kenteken);
                return Ok(voertuig);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("/checkstatus/{id}")]
        public IActionResult GetVoertuigStatus(Guid id)
        {
            try
            {
                var status = _voertuigService.GetVoertuigStatus(id);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpPut("{id}")]
        public IActionResult UpdateVoertuig(Guid id, [FromBody] VoertuigUpDTO DTO)
        {
            try
            {
                _voertuigService.UpdateUpVoertuig(id, DTO);
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
        [HttpPut("veranderGegevens/{id}")]
        public IActionResult veranderGegevens(Guid id, [FromBody] VoertuigWijzigingDTO DTO)
        {
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

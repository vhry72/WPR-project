using Microsoft.AspNetCore.Mvc;
using WPR_project.Models;
using WPR_project.Services;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoertuigStatusController : ControllerBase
    {
        private readonly VoertuigStatusService _service;

        public VoertuigStatusController(VoertuigStatusService voertuigService)
        {
            _service = voertuigService;
        }

        [HttpGet]
        public IActionResult GetAllVoertuigStatussen()
        {
            try
            {
                var voertuigStatuses = _service.GetAllVoertuigStatussen();
                return Ok(voertuigStatuses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }
        [HttpPut("Verhuur/{id}/{verhuurd}")]
        public IActionResult neemIn(Guid id, bool verhuurd)
        {
            try
            {
                var voertuigstatus = _service.GetById(id);
                if (voertuigstatus == null)
                {
                    return NotFound(new { Message = "Voertuig niet gevonden." });
                }
                voertuigstatus.verhuurd = verhuurd;// Update de goedkeuring               
                _service.Update(id, voertuigstatus); 
                return Ok(new { Message = "Voertuig is verhuurd." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }

        [HttpPut("Schade/{id}/{schade}")]
        public IActionResult UpdateSchade(Guid id, bool schade)
        {
            try
            {
                var voertuigstatus = _service.GetById(id);
                if (voertuigstatus == null)
                {
                    return NotFound(new { Message = "Voertuig niet gevonden." });
                }
                voertuigstatus.schade = schade; // Update de schade status
                _service.Update(id, voertuigstatus);
                return Ok(new { Message = "Schade status is bijgewerkt." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }

        [HttpPut("Onderhoud/{id}/{onderhoud}")]
        public IActionResult UpdateOnderhoud(Guid id, bool onderhoud)
        {
            try
            {
                var voertuigstatus = _service.GetById(id);
                if (voertuigstatus == null)
                {
                    return NotFound(new { Message = "Voertuig niet gevonden." });
                }
                voertuigstatus.onderhoud = onderhoud; // Update de onderhoud status
                _service.Update(id, voertuigstatus);
                return Ok(new { Message = "Onderhoud status is bijgewerkt." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }
    }
}

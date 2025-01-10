using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WPR_project.Data;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Services;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchademeldingController : Controller
    {

        private readonly BedrijfsMedewerkersService _service;
        private readonly SchademeldingService _schademeldingservice;

        public SchademeldingController(BedrijfsMedewerkersService service, SchademeldingService schademeldingservice)
        {
            _service = service;
            _schademeldingservice = schademeldingservice;
        }

        // Aanmaken van een nieuwe schademelding
        [HttpPost("maak")]
        public IActionResult CreateSchadeMelding([FromBody] SchademeldingDTO schademelding)
        {
            if (schademelding == null)
            {
                return BadRequest(new { Message = "Invalid data" });
            }

            try
            {
                _service.newSchademelding(schademelding);
                return Ok(new { Message = "Schademelding succesvol aangemaakt" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        

        // Ophalen van alle schademeldingen van een specifiek voertuig
        [HttpGet("{voertuigId}")]
        public IActionResult GetByVoertuig(Guid voertuigId)
        {
            var meldingen = _service.GetSchademeldingByVoertuigId(voertuigId);
            return Ok(meldingen);
        }

        // ophalen van alle schademeldingen
        [HttpGet]
        public ActionResult<IQueryable<SchademeldingDTO>> GetAllSchademeldingen()
        {
            var meldingen = _service.GetAllSchademeldingen();
            return Ok(meldingen);
        }


        //update schademeldingen
        [HttpPut]
        [Route("api/schademeldingen/{id}")]
        public IActionResult UpdateSchademelding(Guid id, [FromBody] SchademeldingDTO DTO)
        {
            try
            {
                _service.updateSchademelding(id, DTO);
                return Ok(new { Message = "Schademelding is succesvol bijgewerkt" });
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
        [HttpPut("inBehandeling/{id}/{status}")]
        public IActionResult zetOpInBehandeling(Guid id, string status)
        {
            try
            {
                var schademelding = _schademeldingservice.GetById(id);
                if (schademelding == null)
                {
                    return NotFound(new { Message = "Huurverzoek niet gevonden." });
                }

                schademelding.Status = "In Behandeling"; // Update de goedkeuring
                

                _schademeldingservice.Update(id, schademelding); 
                return Ok(new { Message = "Schademelding op in behandeling gezet" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }
        [HttpPut("Afgehandeld/{id}/{status}")]
        public IActionResult zetOpAfgehandeld(Guid id, string status)
        {
            try
            {
                var schademelding = _schademeldingservice.GetById(id);
                if (schademelding == null)
                {
                    return NotFound(new { Message = "Huurverzoek niet gevonden." });
                }

                schademelding.Status = "Afgehandeld"; // Update de status


                _schademeldingservice.Update(id, schademelding); // Pas de update correct toe
                return Ok(new { Message = "Schademelding op Afgehandeld gezet" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }
        [HttpPut("InReparatie/{id}/{status}")]
        public IActionResult zetOpInReparatie(Guid id, string status)
        {
            try
            {
                var schademelding = _schademeldingservice.GetById(id);
                if (schademelding == null)
                {
                    return NotFound(new { Message = "Huurverzoek niet gevonden." });
                }

                schademelding.Status = "In Reparatie"; // Update de status
                          

                _schademeldingservice.Update(id, schademelding); // Pas de update correct toe
                return Ok(new { Message = "Schademelding In Repartie" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interne serverfout: {ex.Message}");
            }
        }
        


    }
}






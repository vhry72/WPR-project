using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WPR_project.Data;
using WPR_project.DTO_s;
using WPR_project.Services;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchademeldingController : Controller
    {

        private readonly SchademeldingService _schademeldingservice;

        public SchademeldingController(SchademeldingService schademeldingservice)
        {
            _schademeldingservice = schademeldingservice;
        }

        [Authorize(Roles = "Backofficemedewerker,Frontofficemedewerker")]
        [HttpPost("maak")]
        public IActionResult CreateSchadeMelding([FromBody] SchademeldingDTO schademelding)
        {
            if (schademelding == null)
            {
                return BadRequest(new { Message = "Invalid data" });
            }

            try
            {
                _schademeldingservice.newSchademelding(schademelding);
                return Ok(new { Message = "Schademelding succesvol aangemaakt" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [Authorize(Roles = "Backofficemedewerker,Frontofficemedewerker")]
        [HttpGet]
        public ActionResult<IQueryable<SchadeMeldingInfoDTO>> GetAllSchademeldingen()
        {
            var meldingen = _schademeldingservice.GetAllSchademeldingen();
            return Ok(meldingen);
        }


        [Authorize(Roles = "Backofficemedewerker,Frontofficemedewerker")]
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

        [Authorize(Roles = "Backofficemedewerker,Frontofficemedewerker")]
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

        [Authorize(Roles = "Backofficemedewerker,Frontofficemedewerker")]
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






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

        public SchademeldingController(BedrijfsMedewerkersService service)
        {
            _service = service;
        }

        // Aanmaken van een nieuwe schademelding
        [HttpPost]
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
        public ActionResult<IEnumerable<SchademeldingDTO>> GetAllSchademeldingen()
        {
            var medlingen = _service.GetAllSchademeldingen();
            return Ok(medlingen);
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

    }
}






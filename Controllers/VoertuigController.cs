using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WPR_project.Models;
using WPR_project.Services;

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

        [HttpGet("{id}/status")]
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
    }
}

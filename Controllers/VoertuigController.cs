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
        public IActionResult GetFilteredVoertuigen([FromQuery] string voertuigType/*, [FromQuery] string sorteerOptie*/)
        {
            try
            {
                var voertuigen = _voertuigService.GetFilteredVoertuigen(voertuigType/*, sorteerOptie*/);
                return Ok(voertuigen);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetVoertuigDetails(int id)
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
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using WPR_project.Models;
using WPR_project.Services;
using WPR_project.DTO_s;
using Microsoft.AspNetCore.Authorization;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoertuigNotitiesController : ControllerBase
    {
        private readonly VoertuigNotitiesService _service;

        public VoertuigNotitiesController(VoertuigNotitiesService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Backofficemedewerker,Frontofficemedewerker")]
        [HttpGet("{voertuigId}")]
        public IActionResult GetVoertuigNotities(Guid voertuigId)
        {
            try
            {
                var notities = _service.GetVoertuigNotitiesByVoertuigId(voertuigId);
                if (notities == null)
                {
                    return NotFound("Geen notities gevonden voor het opgegeven voertuig.");
                }
                return Ok(notities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Interne serverfout: " + ex.Message);
            }
        }

        [Authorize(Roles = "Backofficemedewerker,Frontofficemedewerker")]
        [HttpPost]
        public IActionResult AddVoertuigNotitie([FromBody] VoertuigNotitiesDTO voertuigNotitiesDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var voertuigNotities = new VoertuigNotities
                {
                    NotitieId = Guid.NewGuid(),
                    voertuigId = voertuigNotitiesDTO.voertuigId,
                    notitie = voertuigNotitiesDTO.notitie,
                    NotitieDatum = voertuigNotitiesDTO.NotitieDatum
                };

                _service.AddVoertuigNotitie(voertuigNotities);
                return Ok("Notitie succesvol toegevoegd.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Interne serverfout: " + ex.Message);
            }
        }
    }
}

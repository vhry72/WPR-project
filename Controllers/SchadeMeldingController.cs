using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WPR_project.Data;
using WPR_project.Models;

namespace WPR_project.Controllers
{
    public class SchademeldingController : Controller
    {
        private readonly GegevensContext _context;

        public SchademeldingController(GegevensContext context)
        {
            _context = context;
        }

        // Aanmaken van een nieuwe schademelding
        [HttpPost]
        public IActionResult Create(Guid voertuigId, string beschrijving, string status, string opmerkingen)
        {
            var voertuig = _context.Voertuigen.Find(voertuigId);
            if (voertuig == null) return NotFound();

            var schademelding = new Schademelding
            {
                SchademeldingId = Guid.NewGuid(),
                VoertuigId = voertuigId,
                Beschrijving = beschrijving,
                Datum = DateTime.Now,
                Status = status,
                Opmerkingen = opmerkingen
            };

            _context.Schademeldingen.Add(schademelding);
            _context.SaveChanges();

            return Ok(schademelding);
        }

        // Ophalen van alle schademeldingen van een specifiek voertuig
        [HttpGet]
        public IActionResult GetByVoertuig(Guid voertuigId)
        {
            var meldingen = _context.Schademeldingen
                .Where(s => s.VoertuigId == voertuigId)
                .ToList();

            return Ok(meldingen);
        }
        [HttpGet("api/schademeldingen")]
        public IActionResult GetSchademeldingen()
        {
            try
            {
                var meldingen = _context.Schademeldingen.Include(s => s.Voertuig).ToList();
                return Ok(meldingen);
            }
            catch (Exception ex)
            {
                // Log de fout voor verdere analyse
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("api/schademeldingen/{id}")]
        public IActionResult UpdateSchademelding(Guid id, [FromBody] Schademelding updatedMelding)
        {
            var melding = _context.Schademeldingen.Find(id);
            if (melding == null) return NotFound();

            melding.Status = updatedMelding.Status;
            melding.Opmerkingen = updatedMelding.Opmerkingen;

            _context.SaveChanges();
            return Ok(melding);
        }

    }
}

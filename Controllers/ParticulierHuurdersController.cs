using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPR_project.Data;
using WPR_project.DTO_s;
using WPR_project.Models;

namespace WPR_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticulierHuurdersController : ControllerBase
    {
        private readonly GegevensContext _context;

        public ParticulierHuurdersController(GegevensContext context)
        {
            _context = context;
        }

        // GET: api/ParticulierHuurders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParticulierHuurderDTO>>> GetParticulierHuurder()
        {
            return await _context.ParticulierHuurders
                .Select(ph => new ParticulierHuurderDTO
                {
                    particulierId = ph.particulierId,
                    particulierNaam = ph.particulierNaam,
                    particulierEmail = ph.particulierEmail
                })
                .ToListAsync();
        }

        // GET: api/ParticulierHuurders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticulierHuurderDTO>> GetParticulierHuurder(int id)
        {
            var particulierHuurder = await _context.ParticulierHuurders
                .Where(ph => ph.particulierId == id)
                .Select(ph => new ParticulierHuurderDTO
                {
                    particulierId = ph.particulierId,
                    particulierNaam = ph.particulierNaam,
                    particulierEmail = ph.particulierEmail
                })
                .FirstOrDefaultAsync();

            if (particulierHuurder == null)
            {
                return NotFound();
            }

            return particulierHuurder;
        }

        // PUT: api/ParticulierHuurders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticulierHuurder(int id, ParticulierHuurderDTO particulierHuurderDTO)
        {
            if (id != particulierHuurderDTO.particulierId)
            {
                return BadRequest();
            }

            var particulierHuurder = await _context.ParticulierHuurders.FindAsync(id);
            if (particulierHuurder == null)
            {
                return NotFound();
            }

            particulierHuurder.particulierNaam = particulierHuurderDTO.particulierNaam;
            particulierHuurder.particulierEmail = particulierHuurderDTO.particulierEmail;

            _context.Entry(particulierHuurder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticulierHuurderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ParticulierHuurders
        [HttpPost]
        public async Task<ActionResult<ParticulierHuurderDTO>> PostParticulierHuurder(ParticulierHuurderDTO particulierHuurderDTO)
        {
            var particulierHuurder = new ParticulierHuurder
            {
                particulierNaam = particulierHuurderDTO.particulierNaam,
                particulierEmail = particulierHuurderDTO.particulierEmail
            };

            _context.ParticulierHuurders.Add(particulierHuurder);
            await _context.SaveChangesAsync();

            particulierHuurderDTO.particulierId = particulierHuurder.particulierId;

            return CreatedAtAction("GetParticulierHuurder", new { id = particulierHuurderDTO.particulierId }, particulierHuurderDTO);
        }

        // DELETE: api/ParticulierHuurders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticulierHuurder(int id)
        {
            var particulierHuurder = await _context.ParticulierHuurders.FindAsync(id);
            if (particulierHuurder == null)
            {
                return NotFound();
            }

            _context.ParticulierHuurders.Remove(particulierHuurder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParticulierHuurderExists(int id)
        {
            return _context.ParticulierHuurders.Any(e => e.particulierId == id);
        }
    }
}

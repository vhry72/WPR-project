using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPR_project.Data;
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
        public async Task<ActionResult<IEnumerable<ParticulierHuurder>>> GetParticulierHuurder()
        {
            return await _context.ParticulierHuurders.ToListAsync();
        }

        // GET: api/ParticulierHuurders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticulierHuurder>> GetParticulierHuurder(int id)
        {
            var particulierHuurder = await _context.ParticulierHuurders.FindAsync(id);

            if (particulierHuurder == null)
            {
                return NotFound();
            }

            return particulierHuurder;
        }

        // PUT: api/ParticulierHuurders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticulierHuurder(int id, ParticulierHuurder particulierHuurder)
        {
            if (id != particulierHuurder.ParticulierId)
            {
                return BadRequest();
            }

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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParticulierHuurder>> PostParticulierHuurder(ParticulierHuurder particulierHuurder)
        {
            _context.ParticulierHuurders.Add(particulierHuurder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParticulierHuurder", new { id = particulierHuurder.ParticulierId }, particulierHuurder);
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
            return _context.ParticulierHuurders.Any(e => e.ParticulierId == id);
        }
    }
}

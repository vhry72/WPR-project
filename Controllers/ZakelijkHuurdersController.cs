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
    public class ZakelijkHuurdersController : ControllerBase
    {
        private readonly GegevensContext _context;

        public ZakelijkHuurdersController(GegevensContext context)
        {
            _context = context;
        }

        // GET: api/ZakelijkHuurders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ZakelijkHuurder>>> GetZakelijkHuurders()
        {
            return await _context.ZakelijkHuurders.ToListAsync();
        }

        // GET: api/ZakelijkHuurders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ZakelijkHuurder>> GetZakelijkHuurder(int id)
        {
            var zakelijkHuurder = await _context.ZakelijkHuurders.FindAsync(id);

            if (zakelijkHuurder == null)
            {
                return NotFound();
            }

            return zakelijkHuurder;
        }

        // PUT: api/ZakelijkHuurders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZakelijkHuurder(int id, ZakelijkHuurder zakelijkHuurder)
        {
            if (id != zakelijkHuurder.zakelijkeId)
            {
                return BadRequest();
            }

            _context.Entry(zakelijkHuurder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZakelijkHuurderExists(id))
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

        // POST: api/ZakelijkHuurders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ZakelijkHuurder>> PostZakelijkHuurder(ZakelijkHuurder zakelijkHuurder)
        {
            _context.ZakelijkHuurders.Add(zakelijkHuurder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetZakelijkHuurder", new { id = zakelijkHuurder.zakelijkeId }, zakelijkHuurder);
        }

        // DELETE: api/ZakelijkHuurders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZakelijkHuurder(int id)
        {
            var zakelijkHuurder = await _context.ZakelijkHuurders.FindAsync(id);
            if (zakelijkHuurder == null)
            {
                return NotFound();
            }

            _context.ZakelijkHuurders.Remove(zakelijkHuurder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ZakelijkHuurderExists(int id)
        {
            return _context.ZakelijkHuurders.Any(e => e.zakelijkeId == id);
        }
    }
}

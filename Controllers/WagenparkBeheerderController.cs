using Microsoft.AspNetCore.Mvc;
using WPR_project.Models;
using WPR_project.Services;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WagenparkBeheerderController : ControllerBase
    {
        private readonly WagenparkBeheerderService _service;

        public WagenparkBeheerderController(WagenparkBeheerderService service)
        {
            _service = service;
        }

        /// <summary>
        /// Haalt alle wagenparkbeheerders op
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<WagenparkBeheerder>> GetAllBeheerders()
        {
            var beheerders = _service.GetWagenparkBeheerders();
            return Ok(beheerders);
        }

        /// <summary>
        /// Haalt een wagenparkbeheerder op via ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<WagenparkBeheerder> GetBeheerderById(int id)
        {
            var beheerder = _service.GetBeheerderById(id);
            if (beheerder == null)
            {
                return NotFound(new { Message = "Beheerder niet gevonden." });
            }
            return Ok(beheerder);
        }

        /// <summary>
        /// Voegt een nieuwe wagenparkbeheerder toe
        /// </summary>
        [HttpPost]
        public ActionResult AddBeheerder([FromBody] WagenparkBeheerder beheerder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _service.AddWagenparkBeheerder(beheerder);
            return CreatedAtAction(nameof(GetBeheerderById), new { id = beheerder.beheerderId }, beheerder);
        }

        /// <summary>
        /// Wijzigt een bestaande wagenparkbeheerder
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult UpdateBeheerder(int id, [FromBody] WagenparkBeheerder beheerder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _service.UpdateWagenparkBeheerder(id, beheerder);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Verwijdert een wagenparkbeheerder via ID
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteBeheerder(int id)
        {
            try
            {
                _service.DeleteWagenparkBeheerder(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
        /// <summary>
        /// Voegt een medewerker toe aan een wagenparkbeheerder
        /// </summary>
        [HttpPost("{beheerderId}/voegmedewerker")]
        public IActionResult VoegMedewerkerToe(int beheerderId, [FromBody] BedrijfsMedewerkers medewerker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _service.VoegMedewerkerToe(beheerderId, medewerker);
                return Ok(new { Message = "Medewerker succesvol toegevoegd." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Verwijdert een medewerker van een wagenparkbeheerder
        /// </summary>
        [HttpDelete("{beheerderId}/verwijdermedewerker/{medewerkerId}")]
        public IActionResult VerwijderMedewerker(int beheerderId, int medewerkerId)
        {
            try
            {
                _service.VerwijderMedewerker(beheerderId, medewerkerId);
                return Ok(new { Message = "Medewerker succesvol verwijderd." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
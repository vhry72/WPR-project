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
        public ActionResult<WagenparkBeheerder> GetBeheerderById(Guid id)
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
        public ActionResult UpdateBeheerder(Guid id, [FromBody] WagenparkBeheerder beheerder)
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
        public ActionResult DeleteBeheerder(Guid id)
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
        /// Voegt een medewerker toe aan een zakelijke huurder
        /// </summary>
        [HttpPost("{zakelijkeId}/voegmedewerker")]
        public IActionResult VoegMedewerkerToe(Guid zakelijkeId, [FromBody] BedrijfsMedewerkers medewerker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _service.VoegMedewerkerToe(zakelijkeId, medewerker.medewerkerNaam, medewerker.medewerkerEmail);
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
        /// Verwijdert een medewerker van een zakelijke huurder
        /// </summary>
        [HttpDelete("{zakelijkeId}/verwijdermedewerker/{medewerkerId}")]
        public IActionResult VerwijderMedewerker(Guid zakelijkeId, Guid medewerkerId)
        {
            try
            {
                _service.VerwijderMedewerker(zakelijkeId, medewerkerId);
                return Ok(new { Message = "Medewerker succesvol verwijderd." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
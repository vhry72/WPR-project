using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WPR_project.Services;

namespace WPR_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FrontOfficeMedewerkerController : ControllerBase
    {
        private readonly FrontOfficeService _service;

        public FrontOfficeMedewerkerController(FrontOfficeService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllFrontOffice()
        {
            try
            {
                var frontOfficeMedewerkers = _service.GetAll();
                return Ok(frontOfficeMedewerkers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"interne serverfout: {ex.Message}");
            }
        }
    }
}

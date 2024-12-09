//using Microsoft.AspNetCore.Mvc;
//using WPR_project.Services;

//namespace WPR_project.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AbonnementController : ControllerBase
//    {
//        private readonly AbonnementService _service;

//        public AbonnementController(AbonnementService service)
//        {
//            _service = service;
//        }

//        [HttpGet]
//        public IActionResult GetAllAbonnementen()
//        {
//            var abonnementen = _service.GetAllAbonnementen();
//            return Ok(abonnementen);
//        }

//        [HttpPost("{zakelijkeId}/wijzig/{nieuwAbonnementId}")]
//        public IActionResult WijzigAbonnement(Guid zakelijkeId, Guid nieuwAbonnementId)
//        {
//            try
//            {
//                var nieuwAbonnement = _service.GetAllAbonnementen().FirstOrDefault(a => a.AbonnementId == nieuwAbonnementId);
//                if (nieuwAbonnement == null)
//                    return NotFound("Abonnement niet gevonden.");

//                _service.WijzigAbonnement(zakelijkeId, nieuwAbonnementId);

//                return Ok(new
//                {
//                    Message = "Abonnement succesvol gewijzigd.",
//                    Kosten = nieuwAbonnement.Kosten,
//                    Ingangsdatum = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd")
//                });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}

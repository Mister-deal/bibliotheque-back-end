using Microsoft.AspNetCore.Mvc;

namespace bibliotheque_back_end.Controllers
{
    // Pour test TODO: à retirer avant release !
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Ça marche !");
        }
    }
}

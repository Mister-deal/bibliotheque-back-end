using Microsoft.AspNetCore.Mvc;

namespace bibliotheque_back_end.Controllers
{
    public class EmpruntController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

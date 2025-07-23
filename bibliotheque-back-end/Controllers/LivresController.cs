using Microsoft.AspNetCore.Mvc;

namespace bibliotheque_back_end.Controllers
{
    public class LivresController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Gestion des Livres";
            return View();
        }
    }
}

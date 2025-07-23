using Microsoft.AspNetCore.Mvc;

namespace bibliotheque_back_end.Controllers
{
    public class AuteursController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Gestion des Auteurs";
            return View();
        }
    }
}

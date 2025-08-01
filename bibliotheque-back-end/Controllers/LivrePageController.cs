using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace bibliotheque_back_end.Controllers
{
    public class LivrePageController : Controller
    {
        private readonly ILivreService _livreService;

        public LivrePageController(ILivreService livreService)
        {
            _livreService = livreService;
        }

        // GET: /LivrePage/Index - Liste des livres avec recherche (même logique que LivreWebController)
        public async Task<IActionResult> Index(string searchTerm)
        {
            try
            {
                ViewData["CurrentFilter"] = searchTerm;

                var livres = await _livreService.GetAllBooksAsync();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.Trim().ToLower();
                    livres = livres.Where(l =>
                        (l.Titre?.ToLower().Contains(searchTerm) == true) ||
                        (l.Auteur?.ToLower().Contains(searchTerm) == true) ||
                        (l.Categorie.ToString().ToLower().Contains(searchTerm)) ||
                        (l.Editeur?.ToLower().Contains(searchTerm) == true) ||
                        (l.Description?.ToLower().Contains(searchTerm) == true)
                    ).ToList();
                }

                var orderedLivres = livres.OrderBy(l => l.Titre ?? string.Empty).ToList();
                return View(orderedLivres);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors du chargement des livres : {ex.Message}";
                Console.WriteLine($"❌ Erreur dans LivrePageController.Index: {ex.Message}");
                return View(new List<Livre>());
            }
        }

        // GET: /LivrePage/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID du livre doit être supérieur à zéro.");
                }

                var livre = await _livreService.GetBookByIdAsync(id);
                if (livre == null)
                {
                    return NotFound($"Livre avec l'ID {id} non trouvé.");
                }

                // Afficher le message de succès si présent
                if (TempData["SuccessMessage"] != null)
                {
                    ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
                }

                return View(livre);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors du chargement du livre : {ex.Message}";
                Console.WriteLine($"❌ Erreur dans LivrePageController.Details: {ex.Message}");
                return View("Error");
            }
        }

        // GET: /LivrePage/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID du livre doit être supérieur à zéro.");
                }

                var livre = await _livreService.GetBookByIdAsync(id);
                if (livre == null)
                {
                    return NotFound($"Livre avec l'ID {id} non trouvé.");
                }

                return View(livre);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors de la préparation de la modification : {ex.Message}";
                Console.WriteLine($"❌ Erreur dans LivrePageController.Edit: {ex.Message}");
                return View("Error");
            }
        }

        // POST: /LivrePage/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Livre model)
        {
            if (id != model.Id)
            {
                return BadRequest("L'ID de l'URL ne correspond pas à l'ID du livre soumis.");
            }

            if (!ModelState.IsValid)
            {
                // Debug: Afficher les erreurs de validation
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Champ: {error.Key}");
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"  Erreur: {err.ErrorMessage}");
                    }
                }
                return View(model);
            }

            try
            {
                Console.WriteLine($"🔥 DEBUG - Tentative de mise à jour du livre ID: {id}");
                Console.WriteLine($"🔥 Titre: {model.Titre}");
                Console.WriteLine($"🔥 Auteur: {model.Auteur}");
                Console.WriteLine($"🔥 État: {model.Etat}");

                var updatedLivre = await _livreService.UpdateBookAsync(id, model);

                if (updatedLivre == null)
                {
                    return NotFound($"Livre avec l'ID {id} non trouvé après tentative de mise à jour.");
                }

                Console.WriteLine($"✅ Livre mis à jour avec succès !");
                TempData["SuccessMessage"] = "✅ Livre modifié avec succès !";

                return RedirectToAction(nameof(Details), new { id = updatedLivre.Id });
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ ArgumentException: {ex.Message}");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"❌ InvalidOperationException: {ex.Message}");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception générale: {ex.Message}");
                Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
                ModelState.AddModelError(string.Empty, $"Une erreur inattendue est survenue lors de la mise à jour du livre : {ex.Message}");
                return View(model);
            }
        }

        // GET: /LivrePage/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID du livre doit être supérieur à zéro.");
                }

                var livre = await _livreService.GetBookByIdAsync(id);
                if (livre == null)
                {
                    return NotFound($"Livre avec l'ID {id} non trouvé.");
                }

                return View(livre);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors de la préparation de la suppression : {ex.Message}";
                Console.WriteLine($"❌ Erreur dans LivrePageController.Delete: {ex.Message}");
                return View("Error");
            }
        }

        // POST: /LivrePage/DeleteConfirmed/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID du livre doit être supérieur à zéro.");
                }

                Console.WriteLine($"🔥 DEBUG - Tentative de suppression du livre ID: {id}");

                await _livreService.DeleteBookAsync(id);

                Console.WriteLine($"✅ Livre supprimé avec succès !");
                TempData["SuccessMessage"] = "✅ Livre supprimé avec succès !";

                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Livre avec l'ID {id} non trouvé.");
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                var livre = await _livreService.GetBookByIdAsync(id);
                return View(livre);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception lors de la suppression: {ex.Message}");
                ViewBag.ErrorMessage = $"Une erreur inattendue est survenue lors de la suppression du livre : {ex.Message}";
                return View("Error");
            }
        }
    }
}

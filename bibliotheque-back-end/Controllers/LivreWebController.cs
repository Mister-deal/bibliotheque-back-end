using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.Service.Interface;
using BibliothequeBackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace bibliotheque_back_end.Controllers;

public class LivreWebController : Controller // Hérite de Controller, pas de ControllerBase
    {
        private readonly ILivreService _livreService;

        public LivreWebController(ILivreService livreService)
        {
            _livreService = livreService;
        }


        // GET: /LivreWeb/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var livres = await _livreService.GetAllBooksAsync();
                return View(livres);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors du chargement des livres : {ex.Message}";
                return View(new List<Livre>());
            }
        }
        
        // GET: /LivreWeb/Details/{id}
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
                return View(livre);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors de la récupération du livre : {ex.Message}";
                return View("Error");
            }
        }


        // GET: /LivreWeb/Create
        public IActionResult Create()
        {
            return View(new LivreCreateViewModel());
        }

        // POST: /LivreWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Protège contre les attaques CSRF
        public async Task<IActionResult> Create([FromForm] LivreCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var newLivre = new Livre
                {
                    Titre = model.Titre,
                    Auteur = model.Auteur,
                    AnneePublication = model.AnneePublication,
                    Description = model.Description,
                    Editeur = model.Editeur,
                    Etat = model.EtatLivre,
                    Categorie = model.Categorie,
                };

                var createdLivre = await _livreService.AddNewBookAsync(newLivre);

                return RedirectToAction(nameof(Details), new { id = createdLivre.Id });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Une erreur inattendue est survenue lors de la création du livre : {ex.Message}");
                return View(model);
            }
        }


        // GET: /LivreWeb/Edit/{id}
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

                var viewModel = new LivreEditViewModel
                {
                    Id = livre.Id,
                    Titre = livre.Titre,
                    Auteur = livre.Auteur,
                    AnneePublication = livre.AnneePublication,
                };
                return View(viewModel);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors de la préparation de la modification : {ex.Message}";
                return View("Error");
            }
        }

        // POST: /LivreWeb/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] LivreEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("L'ID de l'URL ne correspond pas à l'ID du livre soumis.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var livreToUpdate = new Livre
                {
                    Id = model.Id,
                    Titre = model.Titre,
                    Auteur = model.Auteur,
                    AnneePublication = model.AnneePublication,
                };

                var updatedLivre = await _livreService.UpdateBookAsync(id, livreToUpdate);

                if (updatedLivre == null)
                {
                    return NotFound($"Livre avec l'ID {id} non trouvé après tentative de mise à jour.");
                }

                return RedirectToAction(nameof(Details), new { id = updatedLivre.Id });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Une erreur inattendue est survenue lors de la mise à jour du livre : {ex.Message}");
                return View(model);
            }
        }


        // GET: /LivreWeb/Delete/{id}
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
                return View("Error");
            }
        }

        // POST: /LivreWeb/DeleteConfirmed/{id}
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

                await _livreService.DeleteBookAsync(id);

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
                ViewBag.ErrorMessage = $"Une erreur inattendue est survenue lors de la suppression du livre : {ex.Message}";
                return View("Error");
            }
        }
    }
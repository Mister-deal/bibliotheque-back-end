using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.Service.Interface;
using BibliothequeBackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace bibliotheque_back_end.Controllers.WEB;

public class MembreWebController : Controller
    {
        private readonly IMembreService _membreService;

        public MembreWebController(IMembreService membreService)
        {
            _membreService = membreService;
        }
        
        // GET: /MembreWeb/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var membres = await _membreService.GetAllMembersAsync();
                return View(membres);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors du chargement des membres : {ex.Message}";
                return View(new List<Membre>()); 
            }
        }
        
        // GET: /MembreWeb/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID du membre doit être supérieur à zéro.");
                }

                var membre = await _membreService.GetMemberByIdAsync(id);
                if (membre == null)
                {
                    return NotFound($"Membre avec l'ID {id} non trouvé.");
                }
                return View(membre);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error"); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors de la récupération du membre : {ex.Message}";
                return View("Error");
            }
        }

        // GET: /MembreWeb/Create
        public IActionResult Create()
        {
            return View(new MembreCreateViewModel());
        }

        // POST: /MembreWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] MembreCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            try
            {
                var newMembre = new Membre()
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Email = model.Email,
                    MotDePasse = model.MotDePasse,
                };

                var createdMembre = await _membreService.AddMemberAsync(newMembre, model.MotDePasse);

                return RedirectToAction(nameof(Details), new { id = createdMembre.Id });
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
                ModelState.AddModelError(string.Empty, $"Une erreur inattendue est survenue lors de la création du membre : {ex.Message}");
                return View(model);
            }
        }


        // GET: /MembreWeb/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID du membre doit être supérieur à zéro.");
                }

                var membre = await _membreService.GetMemberByIdAsync(id);
                if (membre == null)
                {
                    return NotFound($"Membre avec l'ID {id} non trouvé.");
                }
                
                var viewModel = new MembreEditViewModel
                {
                    Id = membre.Id,
                    Nom = membre.Nom,
                    Prenom = membre.Prenom,
                    Email = membre.Email
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

        // POST: /MembreWeb/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] MembreEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("L'ID de l'URL ne correspond pas à l'ID du membre soumis.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var existingMembre = await _membreService.GetMemberByIdAsync(id);
                if (existingMembre == null)
                {
                    return NotFound($"Membre avec l'ID {id} non trouvé.");
                }

                existingMembre.Nom = model.Nom;
                existingMembre.Prenom = model.Prenom;
                existingMembre.Email = model.Email;

                if (!string.IsNullOrWhiteSpace(model.NewMotDePasse))
                {
                    existingMembre.MotDePasse = model.NewMotDePasse; 
                }

                var updatedMembre = await _membreService.UpdateMemberAsync(id, existingMembre); 

                if (updatedMembre == null)
                {
                    return NotFound($"Membre avec l'ID {id} non trouvé après tentative de mise à jour.");
                }

                return RedirectToAction(nameof(Details), new { id = updatedMembre.Id });
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
                ModelState.AddModelError(string.Empty, $"Une erreur inattendue est survenue lors de la mise à jour du membre : {ex.Message}");
                return View(model);
            }
        }


        // GET: /MembreWeb/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID du membre doit être supérieur à zéro.");
                }

                var membre = await _membreService.GetMemberByIdAsync(id);
                if (membre == null)
                {
                    return NotFound($"Membre avec l'ID {id} non trouvé.");
                }
                return View(membre);
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

        // POST: /MembreWeb/DeleteConfirmed/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID du membre doit être supérieur à zéro.");
                }

                var deletedMembre = await _membreService.DeleteMemberAsync(id);
                if (deletedMembre == null)
                {
                    return NotFound($"Membre avec l'ID {id} non trouvé.");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                var membre = await _membreService.GetMemberByIdAsync(id);
                return View(membre);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur inattendue est survenue lors de la suppression du membre : {ex.Message}";
                return View("Error");
            }
        }
    }
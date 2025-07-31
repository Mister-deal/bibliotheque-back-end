using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.DTO;
using bibliotheque_back_end.Models.Service.Interface;
using BibliothequeBackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace bibliotheque_back_end.Controllers;

public class EmpruntWebController : Controller
{
    private readonly IEmpruntService _empruntService;
    private readonly IMembreService _membreService;
    private readonly ILivreService _livreService;
    private readonly IEmployeService _employeService;

    public EmpruntWebController(
        IEmpruntService empruntService,
        IMembreService membreService,
        ILivreService livreService,
        IEmployeService employeService)
    {
        _empruntService = empruntService;
        _membreService = membreService;
        _livreService = livreService;
        _employeService = employeService;
    }
    
    // GET: /EmpruntWeb/Index
    public async Task<IActionResult> Index()
    {
        try
        {
            var emprunts = await _empruntService.GetAllEmpruntsAsync();
            return View(emprunts);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Une erreur est survenue lors du chargement des emprunts : {ex.Message}";
            return View(new List<Emprunt>());
        }
    }

    // GET: /EmpruntWeb/ActiveLoans
    public async Task<IActionResult> ActiveLoans()
    {
        try
        {
            var activeEmprunts = await _empruntService.GetActiveEmpruntsAsync();
            return View("Index", activeEmprunts); // Reuse Index view for active loans
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Une erreur est survenue lors du chargement des emprunts actifs : {ex.Message}";
            return View("Index", new List<Emprunt>());
        }
    }
    
    // GET: /EmpruntWeb/Details/{id}
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("L'ID de l'emprunt doit être supérieur à zéro.");
            }

            var emprunt = await _empruntService.GetEmpruntByIdAsync(id);
            if (emprunt == null)
            {
                return NotFound($"Emprunt avec l'ID {id} non trouvé.");
            }

            return View(emprunt);
        }
        catch (ArgumentException ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View("Error");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Une erreur est survenue lors de la récupération de l'emprunt : {ex.Message}";
            return View("Error");
        }
    }
    
    // GET: /EmpruntWeb/Create (Pour afficher le formulaire)
    public async Task<IActionResult> Create()
    {
        // Charger les données pour les dropdowns lors de l'affichage initial du formulaire
        ViewBag.Membres = await _membreService.GetAllMembersAsync(); // Assurez-vous que cette méthode retourne IEnumerable<Membre> ou un DTO
        ViewBag.Livres = await _livreService.GetAllBooksAsync(); // Assurez-vous que cette méthode retourne IEnumerable<Livre> ou un DTO
        ViewBag.Employes = await _employeService.GetAllEmployeesAsync(); // Assurez-vous que cette méthode retourne IEnumerable<Employe> ou un DTO
        return View(new EmpruntCreateViewModel()); // Passe un modèle vide pour le formulaire
    }


    // POST: /EmpruntWeb/Create
    [HttpPost]
    [ValidateAntiForgeryToken] // Protection CSRF pour les formulaires web
    public async Task<IActionResult> Create([FromForm] EmpruntCreateViewModel model)
    {
        // Recharge les données pour les dropdowns avant de vérifier ModelState,
        // afin qu'elles soient disponibles si le modèle n'est pas valide.
        await LoadDropdownsForViewBag();

        if (!ModelState.IsValid)
        {
            return View(model); // Retourne la vue avec les erreurs de validation du ViewModel
        }

        // Mapper le ViewModel vers le DTO de service
        var empruntCreateDto = new EmpruntCreateDto
        {
            MembreId = model.MembreId,
            LivreIds = model.LivreIds ?? new List<int>(), // Assurez-vous que LivreIds n'est jamais null
            DateRetour = model.DateRetour, // Renommé dans le DTO
            EmployeValidationId = model.EmployeValidationId
        };

        try
        {
            // Appeler le service avec le DTO d'entrée
            var createdEmprunt = await _empruntService.CreateEmpruntAsync(empruntCreateDto);

            // Rediriger vers la page de détails du nouvel emprunt créé
            return RedirectToAction(nameof(Details), new { id = createdEmprunt.Id });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            // Pour toute autre erreur inattendue, ajoutez un message générique
            ModelState.AddModelError(string.Empty,
                $"Une erreur inattendue est survenue lors de la création de l'emprunt : {ex.Message}");
            // Loggez l'exception ici pour le débogage en production
            // _logger.LogError(ex, "Erreur lors de la création d'emprunt.");
        }

        // Si une erreur s'est produite (catch block), le code atteint ici.
        // Les ViewBag sont déjà chargés en début de méthode.
        return View(model); // Retourne la vue avec le modèle et les erreurs ajoutées
    }
    
    // Méthode utilitaire pour charger les données des dropdowns
    private async Task LoadDropdownsForViewBag()
    {
        // Assurez-vous que ces méthodes de service retournent bien des collections pour les dropdowns
        ViewBag.Membres = await _membreService.GetAllMembersAsync();
        ViewBag.Livres = await _livreService.GetAllBooksAsync();
        ViewBag.Employes = await _employeService.GetAllEmployeesAsync();
    }

    
    // GET: /EmpruntWeb/ReturnBook/{empruntId}/{livreId} (Optional, could be combined with ReturnAll)
    public async Task<IActionResult> ReturnBook(int empruntId, int livreId)
    {
        try
        {
            var emprunt = await _empruntService.GetEmpruntByIdAsync(empruntId);
            if (emprunt == null) return NotFound($"Emprunt avec l'ID {empruntId} non trouvé.");

            var livre = emprunt.LivresEmpruntes.FirstOrDefault(el => el.IdLivre == livreId)?.livre;
            if (livre == null) return NotFound($"Livre avec l'ID {livreId} non trouvé dans cet emprunt.");

            ViewBag.Employes = await _employeService.GetAllEmployeesAsync();
            return View(new ReturnSpecificBookViewModel
                { EmpruntId = empruntId, LivreId = livreId, LivreTitre = livre.Titre });
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Erreur lors de la préparation du retour : {ex.Message}";
            return View("Error");
        }
    }

    // POST: /EmpruntWeb/ReturnBook
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReturnBook([FromForm] ReturnSpecificBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Employes = await _employeService.GetAllEmployeesAsync();
            return View(model);
        }

        try
        {
            var updatedEmprunt = await _empruntService.ReturnSpecificBookFromEmpruntAsync(
                model.EmpruntId, model.LivreId, model.EmployeValidationId
            );

            if (updatedEmprunt == null)
                return NotFound($"Emprunt {model.EmpruntId} non trouvé ou livre {model.LivreId} non associé.");

            // Redirect to the loan details or the loan list
            return RedirectToAction(nameof(Details), new { id = updatedEmprunt.Id });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty,
                $"Une erreur inattendue est survenue lors du retour du livre : {ex.Message}");
        }

        ViewBag.Employes = await _employeService.GetAllEmployeesAsync();
        return View(model);
    }

    // GET: /EmpruntWeb/ReturnAll/{empruntId}
    public async Task<IActionResult> ReturnAll(int empruntId)
    {
        try
        {
            var emprunt = await _empruntService.GetEmpruntByIdAsync(empruntId);
            if (emprunt == null) return NotFound($"Emprunt avec l'ID {empruntId} non trouvé.");

            if (emprunt.DateRetour.HasValue)
            {
                ViewBag.ErrorMessage = "Cet emprunt est déjà marqué comme entièrement retourné.";
                return View("Details", emprunt);
            }

            ViewBag.Employes = await _employeService.GetAllEmployeesAsync();
            return View(new ReturnAllBooksViewModel
            {
                EmpruntId = empruntId,
                EmpruntDetails =
                    $"Emprunt ID: {emprunt.Id} par ce membre contient ({emprunt.LivresEmpruntes.Count} livres)"
            });
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Erreur lors de la préparation du retour de tous les livres : {ex.Message}";
            return View("Error");
        }
    }

    // POST: /EmpruntWeb/ReturnAll
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReturnAll([FromForm] ReturnAllBooksViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Employes = await _employeService.GetAllEmployeesAsync();
            return View(model);
        }

        try
        {
            var updatedEmprunt = await _empruntService.ReturnAllBooksForEmpruntAsync(
                model.EmpruntId, model.EmployeValidationId
            );

            if (updatedEmprunt == null) return NotFound($"Emprunt {model.EmpruntId} non trouvé.");

            return RedirectToAction(nameof(Details), new { id = updatedEmprunt.Id });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty,
                $"Une erreur inattendue est survenue lors du retour de tous les livres : {ex.Message}");
        }

        ViewBag.Employes = await _employeService.GetAllEmployeesAsync();
        return View(model);
    }
    
    // GET: /EmpruntWeb/Delete/{id}
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("L'ID de l'emprunt doit être supérieur à zéro.");
            }

            var emprunt = await _empruntService.GetEmpruntByIdAsync(id);
            if (emprunt == null)
            {
                return NotFound($"Emprunt avec l'ID {id} non trouvé.");
            }
            
            if (!emprunt.DateRetour.HasValue) 
            {
                ViewBag.ErrorMessage = "Impossible de supprimer un emprunt qui n'est pas entièrement retourné.";
                return View("Details", emprunt);
            }

            return View(emprunt);
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

    // POST: /EmpruntWeb/DeleteConfirmed/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("L'ID de l'emprunt doit être supérieur à zéro.");
            }

            var deletedEmprunt = await _empruntService.DeleteEmpruntAsync(id);
            if (deletedEmprunt == null)
            {
                return NotFound($"Emprunt avec l'ID {id} non trouvé.");
            }

            return RedirectToAction(nameof(Index)); // Redirect to the list after successful deletion
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty,
                $"Une erreur inattendue est survenue lors de la suppression de l'emprunt : {ex.Message}");
        }

        // If an error occurred, retrieve the loan again to display the confirmation page with errors
        var emprunt = await _empruntService.GetEmpruntByIdAsync(id);
        return View(emprunt);
    }
}
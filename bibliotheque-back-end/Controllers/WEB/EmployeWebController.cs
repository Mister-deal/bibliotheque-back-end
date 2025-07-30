using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.Service.Interface;
using BibliothequeBackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace bibliotheque_back_end.Controllers.WEB

{
    // Ce contrôleur est dédié aux vues Razor (HTML)
    // Il n'a PAS [ApiController] et n'est PAS dans le chemin "api/" par défaut.
    public class EmployeWebController : Controller
    {
        private readonly IEmployeService _employeService;

        public EmployeWebController(IEmployeService employeService)
        {
            _employeService = employeService;
        }

        // GET: /EmployeWeb/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var employes = await _employeService.GetAllEmployeesAsync();
                return View(employes); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors du chargement des employés : {ex.Message}";
                return View(new List<Employe>()); 
            }
        }


        // GET: /EmployeWeb/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID de l'employé doit être supérieur à zéro.");
                }

                var employe = await _employeService.GetEmployeeByIdAsync(id);
                if (employe == null)
                {
                    return NotFound($"Employé avec l'ID {id} non trouvé.");
                }

                return View(employe);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error"); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Une erreur est survenue lors de la récupération de l'employé : {ex.Message}";
                return View("Error");
            }
        }


        // GET: /EmployeWeb/Create
        public IActionResult Create()
        {
            return View(new EmployeCreateViewModel()); 
        }

        // POST: /EmployeWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([FromForm] EmployeCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            try
            {
                var newEmploye = new Employe
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Email = model.Email,
                    Role = model.Role 
                };

                var createdEmploye = await _employeService.AddEmployeeAsync(newEmploye, model.MotDePasse);

                return RedirectToAction(nameof(Details), new { id = createdEmploye.Id });
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
                ModelState.AddModelError(string.Empty, $"Une erreur inattendue est survenue : {ex.Message}");
                return View(model);
            }
        }


        // GET: /EmployeWeb/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID de l'employé doit être supérieur à zéro.");
                }

                var employe = await _employeService.GetEmployeeByIdAsync(id);
                if (employe == null)
                {
                    return NotFound($"Employé avec l'ID {id} non trouvé.");
                }

                var viewModel = new EmployeEditViewModel
                {
                    Id = employe.Id,
                    Nom = employe.Nom,
                    Prenom = employe.Prenom,
                    Email = employe.Email,
                    Role = employe.Role
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
                ViewBag.ErrorMessage =
                    $"Une erreur est survenue lors de la préparation de la modification : {ex.Message}";
                return View("Error");
            }
        }

        // POST: /EmployeWeb/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] EmployeEditViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("L'ID de l'URL ne correspond pas à l'ID de l'employé soumis.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var existingEmploye = await _employeService.GetEmployeeByIdAsync(id);
                if (existingEmploye == null)
                {
                    return NotFound($"Employé avec l'ID {id} non trouvé.");
                }

                existingEmploye.Nom = model.Nom;
                existingEmploye.Prenom = model.Prenom;
                existingEmploye.Email = model.Email;
                existingEmploye.Role = model.Role;

                if (!string.IsNullOrWhiteSpace(model.NewMotDePasse))
                {
                    existingEmploye.MotDePasse =
                        model.NewMotDePasse;
                }

                var updatedEmploye =
                    await _employeService.UpdateEmployeeAsync(id,
                        existingEmploye);

                if (updatedEmploye == null)
                {
                    return NotFound($"Employé avec l'ID {id} non trouvé après tentative de mise à jour.");
                }

                return RedirectToAction(nameof(Details), new { id = updatedEmploye.Id });
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
                ModelState.AddModelError(string.Empty,
                    $"Une erreur inattendue est survenue lors de la mise à jour de l'employé : {ex.Message}");
                return View(model);
            }
        }


        // GET: /EmployeWeb/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID de l'employé doit être supérieur à zéro.");
                }

                var employe = await _employeService.GetEmployeeByIdAsync(id);
                if (employe == null)
                {
                    return NotFound($"Employé avec l'ID {id} non trouvé.");
                }

                return View(employe);
            }
            catch (ArgumentException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage =
                    $"Une erreur est survenue lors de la préparation de la suppression : {ex.Message}";
                return View("Error");
            }
        }

        // POST: /EmployeWeb/DeleteConfirmed/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("L'ID de l'employé doit être supérieur à zéro.");
                }

                var deletedEmploye = await _employeService.DeleteEmployeeAsync(id);
                if (deletedEmploye == null)
                {
                    return NotFound($"Employé avec l'ID {id} non trouvé.");
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
                var employe = await _employeService.GetEmployeeByIdAsync(id);
                return View(employe); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage =
                    $"Une erreur inattendue est survenue lors de la suppression de l'employé : {ex.Message}";
                return View("Error");
            }
        }
    }
}
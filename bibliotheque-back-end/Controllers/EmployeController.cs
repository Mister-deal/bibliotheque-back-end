using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Controllers;

    [ApiController]
[Route("api/[controller]")]
[SwaggerTag("Contrôleur de gestion des employés de la bibliothèque")]
public class EmployeController : ControllerBase
{
    private readonly IEmployeService _employeService;

    public EmployeController(IEmployeService employeService)
    {
        _employeService = employeService;
    }
    //## Récupérer tous les employés

    //testé
    // GET: api/Employes
    [HttpGet]
    [SwaggerOperation(
        Summary = "Récupère tous les employés",
        Description = "Retourne la liste complète des employés enregistrés dans la base de données."
    )]
    [SwaggerResponse(200, "Liste des employés retournée avec succès.", typeof(IEnumerable<Employe>))]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la récupération des employés.")]
    public async Task<ActionResult<IEnumerable<Employe>>> GetEmployes()
    {
        try
        {
            var employes = await _employeService.GetAllEmployeesAsync();
            return Ok(employes);
        }
        catch (Exception ex)
        {
            // Il est recommandé de logger l'exception ici pour le débogage.
            return StatusCode(500, $"Une erreur interne est survenue lors de la récupération des employés : {ex.Message}");
        }
    }
    //## Récupérer un employé par ID

    //testé
    // GET: api/Employes/{id}
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Récupère un employé par son identifiant",
        Description = "Permet de récupérer un employé spécifique via son identifiant unique."
    )]
    [SwaggerResponse(200, "Employé trouvé.", typeof(Employe))]
    [SwaggerResponse(404, "Employé non trouvé.")]
    [SwaggerResponse(400, "Requête invalide (ID négatif ou zéro).")]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la récupération de l'employé.")]
    public async Task<ActionResult<Employe>> GetEmployeById(int id)
    {
        try
        {
            var employe = await _employeService.GetEmployeeByIdAsync(id);
            if (employe == null)
            {
                return NotFound($"Employé avec l'ID {id} non trouvé.");
            }
            return Ok(employe);
        }
        catch (ArgumentException ex) // Capturer l'exception pour l'ID <= 0 du service
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de la récupération de l'employé : {ex.Message}");
        }
    }
    //## Récupérer un employé par email

    //testé
    // GET: api/Employes/byEmail?email={email}
    [HttpGet("byEmail")]
    [SwaggerOperation(
        Summary = "Récupère un employé par son adresse email",
        Description = "Permet de récupérer un employé spécifique via son adresse email."
    )]
    [SwaggerResponse(200, "Employé trouvé.", typeof(Employe))]
    [SwaggerResponse(404, "Employé non trouvé.")]
    [SwaggerResponse(400, "Requête invalide (email vide ou nul).")]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la récupération de l'employé.")]
    public async Task<ActionResult<Employe>> GetEmployeByEmail([FromQuery] string email)
    {
        try
        {
            var employe = await _employeService.GetEmployeeByEmailAsync(email);
            if (employe == null)
            {
                return NotFound($"Employé avec l'email '{email}' non trouvé.");
            }
            return Ok(employe);
        }
        catch (ArgumentException ex) // Capturer l'exception pour l'email vide/nul du service
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de la récupération de l'employé par email : {ex.Message}");
        }
    }
    //## Ajouter un nouvel employé

    //testé
    // POST: api/Employes
    [HttpPost]
    [SwaggerOperation(
        Summary = "Ajoute un nouvel employé",
        Description = "Crée un nouvel employé dans la base de données. Le mot de passe fourni via 'dataPassword' sera hashé avant d'être stocké."
    )]
    [SwaggerResponse(201, "Employé créé avec succès.", typeof(Employe))]
    [SwaggerResponse(400, "Requête invalide (données manquantes, incorrectes ou employé déjà existant).")]
    [SwaggerResponse(409, "Conflit : Un employé avec le même email existe déjà.")]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la création de l'employé.")]
    public async Task<ActionResult<Employe>> PostEmploye(
        [FromBody] Employe employe,
        [FromQuery(Name = "dataPassword")] string dataPassword)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdEmploye = await _employeService.AddEmployeeAsync(employe, dataPassword);
            // Utilise nameof(GetEmployeById) pour la route GET appropriée.
            return CreatedAtAction(nameof(GetEmployeById), new { id = createdEmploye.Id }, createdEmploye);
        }
        catch (ArgumentException ex) // Données invalides (email, nom, prénom, mot de passe, rôle manquants/invalides)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex) // Employé avec le même email déjà existant
        {
            return Conflict(ex.Message); // Utilisez 409 Conflict pour les doublons
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de l'ajout de l'employé : {ex.Message}");
        }
    }
    //## Mettre à jour un employé existant

    //a tester
    // PUT: api/Employes/{id}
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Met à jour un employé existant",
        Description = "Modifie les informations d'un employé existant à partir de son identifiant. " +
                      "Le mot de passe peut être mis à jour en l'incluant dans le corps de la requête."
    )]
    [SwaggerResponse(200, "Employé mis à jour avec succès.", typeof(Employe))]
    [SwaggerResponse(404, "Employé non trouvé.")]
    [SwaggerResponse(400, "Requête invalide (données manquantes, incorrectes ou ID mismatched).")]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la mise à jour de l'employé.")]
    public async Task<ActionResult<Employe>> PutEmploye(int id, [FromBody] Employe employe)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Le service gère déjà la validation id != updatedEmployee.Id, mais une vérification précoce ici peut être utile.
        if (id != employe.Id)
        {
            return BadRequest("L'ID de l'employé dans l'URL ne correspond pas à l'ID dans le corps de la requête.");
        }

        try
        {
            var updatedEmploye = await _employeService.UpdateEmployeeAsync(id, employe);
            if (updatedEmploye == null)
            {
                return NotFound($"Employé avec l'ID {id} non trouvé.");
            }
            return Ok(updatedEmploye);
        }
        catch (ArgumentException ex) // Pour les validations d'ID ou de données
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de la mise à jour de l'employé : {ex.Message}");
        }
    }
    //## Supprimer un employé

    //testé
    // DELETE: api/Employes/{id}
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Supprime un employé par son identifiant",
        Description = "Supprime définitivement un employé à partir de son identifiant."
    )]
    [SwaggerResponse(200, "Employé supprimé avec succès.", typeof(Employe))] // 200 OK avec l'objet supprimé
    [SwaggerResponse(404, "Employé non trouvé.")]
    [SwaggerResponse(400, "Requête invalide (règles métier empêchant la suppression, ex: employé a des responsabilités actives).")]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la suppression de l'employé.")]
    public async Task<ActionResult<Employe>> DeleteEmploye(int id)
    {
        try
        {
            var deletedEmploye = await _employeService.DeleteEmployeeAsync(id);
            if (deletedEmploye == null)
            {
                return NotFound($"Employé avec l'ID {id} non trouvé.");
            }
            return Ok(deletedEmploye); // Retourne l'employé supprimé avec 200 OK
        }
        catch (ArgumentException ex) // ID <= 0
        {
            return BadRequest(ex.Message);
        }
        // Ajoutez ici d'autres catch pour des InvalidOperationException si votre service en lance
        // pour des règles métier spécifiques (ex: ne pas supprimer un employé qui a encore des emprunts actifs à gérer ou est responsable d'autres entités).
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de la suppression de l'employé : {ex.Message}");
        }
    }
}
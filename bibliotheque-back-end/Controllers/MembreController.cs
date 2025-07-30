using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Controllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Contrôleur de gestion des membres de la bibliothèque")]
public class MembreController : ControllerBase
{
    private readonly IMembreService _membreService;

    public MembreController(IMembreService membreService)
    {
        _membreService = membreService;
    }

    //## Récupérer tous les membres
    [HttpGet]
    [SwaggerOperation(
        Summary = "Récupère tous les membres",
        Description = "Retourne la liste complète des membres enregistrés dans la base de données."
    )]
    [SwaggerResponse(200, "Liste des membres retournée avec succès.", typeof(IEnumerable<Membre>))]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la récupération des membres.")]
    public async Task<ActionResult<IEnumerable<Membre>>> GetMembres()
    {
        try
        {
            var membres = await _membreService.GetAllMembersAsync();
            return Ok(membres);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de la récupération des membres : {ex.Message}");
        }
    }

    //## Récupérer un membre par ID
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Récupère un membre par son identifiant",
        Description = "Permet de récupérer un membre spécifique via son identifiant unique."
    )]
    [SwaggerResponse(200, "Membre trouvé.", typeof(Membre))]
    [SwaggerResponse(404, "Membre non trouvé.")]
    [SwaggerResponse(400, "Requête invalide (ID négatif ou zéro).")]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la récupération du membre.")]
    public async Task<ActionResult<Membre>> GetMembreById(int id)
    {
        try
        {
            var membre = await _membreService.GetMemberByIdAsync(id);
            if (membre == null)
            {
                return NotFound($"Membre avec l'ID {id} non trouvé.");
            }
            return Ok(membre);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de la récupération du membre : {ex.Message}");
        }
    }

    //## Récupérer un membre par email
    [HttpGet("byEmail")]
    [SwaggerOperation(
        Summary = "Récupère un membre par son adresse email",
        Description = "Permet de récupérer un membre spécifique via son adresse email."
    )]
    [SwaggerResponse(200, "Membre trouvé.", typeof(Membre))]
    [SwaggerResponse(404, "Membre non trouvé.")]
    [SwaggerResponse(400, "Requête invalide (email vide ou nul).")]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la récupération du membre.")]
    public async Task<ActionResult<Membre>> GetMembreByEmail([FromQuery] string email)
    {
        try
        {
            var membre = await _membreService.GetMemberByEmailAsync(email);
            if (membre == null)
            {
                return NotFound($"Membre avec l'email '{email}' non trouvé.");
            }
            return Ok(membre);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de la récupération du membre par email : {ex.Message}");
        }
    }

    //## Ajouter un nouveau membre
    [HttpPost]
    [SwaggerOperation(
        Summary = "Ajoute un nouveau membre",
        Description = "Crée un nouveau membre dans la base de données. Le mot de passe fourni via 'dataPassword' sera hashé avant d'être stocké."
    )]
    [SwaggerResponse(201, "Membre créé avec succès.", typeof(Membre))]
    [SwaggerResponse(400, "Requête invalide (données manquantes, incorrectes ou membre déjà existant).")]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la création du membre.")]
    public async Task<ActionResult<Membre>> PostMembre(
        [FromBody] Membre membre,
        [FromQuery(Name = "dataPassword")] string dataPassword)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdMember = await _membreService.AddMemberAsync(membre, dataPassword);
            return CreatedAtAction(nameof(GetMembreById), new { id = createdMember.Id }, createdMember);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de l'ajout du membre : {ex.Message}");
        }
    }

    //## Mettre à jour un membre existant
    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Met à jour un membre existant",
        Description = "Modifie les informations d'un membre existant à partir de son identifiant. " +
                      "Le mot de passe peut être mis à jour en l'incluant dans le corps de la requête."
    )]
    [SwaggerResponse(200, "Membre mis à jour avec succès.", typeof(Membre))]
    [SwaggerResponse(404, "Membre non trouvé.")]
    [SwaggerResponse(400, "Requête invalide (données manquantes, incorrectes ou ID mismatched).")]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la mise à jour du membre.")]
    public async Task<ActionResult<Membre>> PutMembre(int id, [FromBody] Membre membre)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != membre.Id)
        {
            return BadRequest("L'ID du membre dans l'URL ne correspond pas à l'ID dans le corps de la requête.");
        }

        try
        {
            var updatedMember = await _membreService.UpdateMemberAsync(id, membre);
            if (updatedMember == null)
            {
                return NotFound($"Membre avec l'ID {id} non trouvé.");
            }
            return Ok(updatedMember);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de la mise à jour du membre : {ex.Message}");
        }
    }

    //## Supprimer un membre
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Supprime un membre par son identifiant",
        Description = "Supprime définitivement un membre à partir de son identifiant."
    )]
    [SwaggerResponse(200, "Membre supprimé avec succès.", typeof(Membre))]
    [SwaggerResponse(404, "Membre non trouvé.")]
    [SwaggerResponse(400, "Requête invalide (membre ayant des emprunts actifs).")]
    [SwaggerResponse(500, "Erreur interne du serveur lors de la suppression du membre.")]
    public async Task<ActionResult<Membre>> DeleteMembre(int id)
    {
        try
        {
            var deletedMember = await _membreService.DeleteMemberAsync(id);
            if (deletedMember == null)
            {
                return NotFound($"Membre avec l'ID {id} non trouvé.");
            }
            return Ok(deletedMember);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Une erreur interne est survenue lors de la suppression du membre : {ex.Message}");
        }
    }
}

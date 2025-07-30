using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Contrôleur de gestion des livres de la bibliothèque")]
    public class LivresController : ControllerBase
    {
        private readonly ILivreService _livreService;

        public LivresController(ILivreService livreService)
        {
            _livreService = livreService;
        }

        //testé
        // GET : api/livres
        [HttpGet]
        [SwaggerOperation(
            Summary = "Récupère tous les livres",
            Description = "Retourne la liste complète des livres enregistrés en base de données"
        )]
        [SwaggerResponse(200, "Liste des livres retournée avec succès", typeof(IEnumerable<Livre>))]
        public async Task<ActionResult<IEnumerable<Livre>>> GetLivres() // <-- Changement ici : async Task<...>
        {
            var livres = await _livreService.GetAllBooksAsync(); // <-- Utilisation de await
            return Ok(livres);
        }

        //testé
        // GET: api/livres/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Récupère un livre par son identifiant",
            Description = "Permet de récupérer un livre spécifique via son identifiant unique"
        )]
        [SwaggerResponse(200, "Livre trouvé", typeof(Livre))]
        [SwaggerResponse(404, "Livre non trouvé")]
        [SwaggerResponse(400, "Requête invalide (ID négatif ou zéro)")] // Ajout du 400
        public async Task<ActionResult<Livre>> GetLivre(int id) // <-- Changement ici : async Task<...>
        {
            try
            {
                var livre = await _livreService.GetBookByIdAsync(id); // <-- Utilisation de await
                if (livre == null) // <-- Vérification explicite de null
                {
                    return NotFound();
                }
                return Ok(livre);
            }
            // KeyNotFoundException ne sera plus lancée par le service pour ce cas,
            // mais gardez-la si d'autres méthodes de service (ou futures) peuvent la lancer.
            // Pour GetBookByIdAsync, le service retourne null, ce qui est géré ci-dessus.
            // catch (KeyNotFoundException)
            // {
            //     return NotFound();
            // }
            catch (ArgumentException ex) // Le service lance toujours ArgumentException pour les IDs <= 0
            {
                return BadRequest(ex.Message);
            }
        }

        //testé
        // POST: api/livres
        [HttpPost]
        [SwaggerOperation(
            Summary = "Ajoute un nouveau livre",
            Description = "Permet de créer un nouveau livre dans la base de données"
        )]
        [SwaggerResponse(201, "Livre créé avec succès", typeof(Livre))]
        [SwaggerResponse(400, "Requête invalide (données manquantes ou incorrectes, ou livre déjà existant)")] // Ajustement du message
        public async Task<ActionResult<Livre>> PostLivre([FromBody] Livre livre) // <-- Changement ici : async Task<...>
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _livreService.AddNewBookAsync(livre); // <-- Utilisation de await
                return CreatedAtAction(nameof(GetLivre), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex) // Pour gérer le cas où un livre avec le même titre/auteur/éditeur existe déjà
            {
                return BadRequest(ex.Message);
            }
        }

        //a tester
        // PUT: api/livres/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Met à jour un livre existant",
            Description = "Modifie toutes les propriétés d’un livre existant à partir de son identifiant"
        )]
        [SwaggerResponse(200, "Livre mis à jour avec succès", typeof(Livre))] // Changement de 204 à 200 pour retourner l'objet mis à jour
        [SwaggerResponse(404, "Livre non trouvé")]
        [SwaggerResponse(400, "Requête invalide (données manquantes ou incorrectes, ou ID mismatched)")]
        public async Task<ActionResult<Livre>> PutLivre(int id, [FromBody] Livre livre) // <-- Changement ici : async Task<ActionResult<Livre>>
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // On s’aligne sur l’id de la route (et on ignore tout id dans le body)
                // Note: La validation id != updatedBook.Id est maintenant gérée dans le service,
                // donc l'assignation ici est acceptable.
                livre.Id = id; 
                var updated = await _livreService.UpdateBookAsync(id, livre); // <-- Utilisation de await

                if (updated == null) // <-- Vérification explicite de null
                {
                    return NotFound();
                }
                return Ok(updated); // Retourne l'objet mis à jour (plus informatif que NoContent)
            }
            // KeyNotFoundException ne sera plus lancée par le service pour ce cas.
            // catch (KeyNotFoundException) { return NotFound(); }
            catch (ArgumentException ex) // Pour les validations d'ID ou de données
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex) // Si d'autres règles métier entraînent cela
            {
                return BadRequest(ex.Message);
            }
        }

        //testé
        // DELETE: api/livres/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Supprime un livre par son identifiant",
            Description = "Supprime définitivement un livre à partir de son identifiant"
        )]
        [SwaggerResponse(204, "Livre supprimé avec succès")]
        [SwaggerResponse(404, "Livre non trouvé")]
        [SwaggerResponse(400, "Requête invalide (livre emprunté ou autres contraintes)")] // Ajustement du message
        public async Task<IActionResult> DeleteLivre(int id) // <-- Changement ici : async Task<...>
        {
            try
            {
                await _livreService.DeleteBookAsync(id); // <-- Utilisation de await
                return NoContent();
            }
            catch (KeyNotFoundException) // Le service peut encore lancer KeyNotFoundException pour DELETE
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) // Pour les règles métier comme "livre emprunté"
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex) // Pour les IDs <= 0
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
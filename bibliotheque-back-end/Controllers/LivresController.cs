using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.Service.Interface;
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

        // GET : api/livres
        [HttpGet]
        [SwaggerOperation(
            Summary = "Récupère tous les livres",
            Description = "Retourne la liste complète des livres enregistrés en base de données"
        )]
        [SwaggerResponse(200, "Liste des livres retournée avec succès", typeof(IEnumerable<Livre>))]
        public ActionResult<IEnumerable<Livre>> GetLivres()
        {
            var livres = _livreService.GetAllBooksAsync();
            return Ok(livres);
        }

        // GET: api/livres/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Récupère un livre par son identifiant",
            Description = "Permet de récupérer un livre spécifique via son identifiant unique"
        )]
        [SwaggerResponse(200, "Livre trouvé", typeof(Livre))]
        [SwaggerResponse(404, "Livre non trouvé")]
        public ActionResult<Livre> GetLivre(int id)
        {
            try
            {
                var livre = _livreService.GetBookByIdAsync(id);
                return Ok(livre);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/livres
        [HttpPost]
        [SwaggerOperation(
            Summary = "Ajoute un nouveau livre",
            Description = "Permet de créer un nouveau livre dans la base de données"
        )]
        [SwaggerResponse(201, "Livre créé avec succès", typeof(Livre))]
        [SwaggerResponse(400, "Requête invalide (données manquantes ou incorrectes)")]
        public ActionResult<Livre> PostLivre([FromBody] Livre livre)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = _livreService.AddNewBookAsync(livre);
                return CreatedAtAction(nameof(GetLivre), new { id = created.Id }, created);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        // PUT: api/livres/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Met à jour un livre existant",
            Description = "Modifie toutes les propriétés d’un livre existant à partir de son identifiant"
        )]
        [SwaggerResponse(204, "Livre mis à jour avec succès")]
        [SwaggerResponse(404, "Livre non trouvé")]
        [SwaggerResponse(400, "Requête invalide (données manquantes ou incorrectes)")]
        public IActionResult PutLivre(int id, [FromBody] Livre livre)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // on s’aligne sur l’id de la route (et on ignore tout id dans le body)
                livre.Id = id;
                _livreService.UpdateBookAsync(id, livre);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        // DELETE: api/livres/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Supprime un livre par son identifiant",
            Description = "Supprime définitivement un livre à partir de son identifiant"
        )]
        [SwaggerResponse(204, "Livre supprimé avec succès")]
        [SwaggerResponse(404, "Livre non trouvé")]
        public IActionResult DeleteLivre(int id)
        {
            try
            {
                _livreService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

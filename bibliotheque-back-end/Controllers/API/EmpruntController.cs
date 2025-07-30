using System.ComponentModel.DataAnnotations;
using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Contrôleur de gestion des emprunts")]
    public class EmpruntsController : ControllerBase
    {
        private readonly IEmpruntService _empruntService;

        public EmpruntsController(IEmpruntService empruntService)
        {
            _empruntService = empruntService;
        }

        // Petit contrat d'entrée pour POST (pas un DTO métier)
        public class CreateEmpruntRequest
        {
            [Required] public int MembreId { get; set; }

            [Required, MinLength(1)]
            public List<int> LivreIds { get; set; } = new();

            // ISO 8601: "2025-07-29"
            public DateOnly? DateRetourPrevue { get; set; }

            [Required] public int EmployeValidationId { get; set; }
        }

        // GET: api/emprunts
        [HttpGet]
        [SwaggerOperation(Summary = "Liste tous les emprunts")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Emprunt>))]
        public async Task<ActionResult<IEnumerable<Emprunt>>> GetAll()
            => Ok(_empruntService.GetAllEmpruntsAsync());

        // GET: api/emprunts/actifs
        [HttpGet("actifs")]
        [SwaggerOperation(Summary = "Liste les emprunts actifs (non clôturés)")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Emprunt>))]
        public async Task<ActionResult<IEnumerable<Emprunt>>> GetActifs()
        {
            var activeEmprunts = await _empruntService.GetActiveEmpruntsAsync();
            return Ok(activeEmprunts);
        }

        // GET: api/emprunts/{id}
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Récupère un emprunt par son Id")]
        [SwaggerResponse(200, "OK", typeof(Emprunt))]
        [SwaggerResponse(404, "Emprunt non trouvé")]
        public async Task<ActionResult<Emprunt>> GetById(int id)
        {
            try
            {
                var e = _empruntService.GetEmpruntByIdAsync(id);
                return Ok(e);
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        // GET: api/emprunts/membre/{membreId}
        [HttpGet("membre/{membreId:int}")]
        [SwaggerOperation(Summary = "Récupère les emprunts d’un membre")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Emprunt>))]
        public async Task<ActionResult<IEnumerable<Emprunt>>> GetByMembre(int membreId)
        {
            try
            {
                return Ok(_empruntService.GetEmpruntsByMembreIdAsync(membreId));
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        // GET: api/emprunts/livre/{livreId}
        [HttpGet("livre/{livreId:int}")]
        [SwaggerOperation(Summary = "Récupère les emprunts contenant un livre donné")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Emprunt>))]
        public async Task<ActionResult<IEnumerable<Emprunt>>> GetByLivre(int livreId)
        {
            try
            {
                return Ok(_empruntService.GetEmpruntsByLivreIdAsync(livreId));
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        // POST: api/emprunts
        [HttpPost]
        [SwaggerOperation(
            Summary = "Crée un nouvel emprunt",
            Description = "Crée un emprunt pour un membre avec une liste de livres. Les livres passent en état 'Emprunte'.")]
        [SwaggerResponse(201, "Créé", typeof(Emprunt))]
        [SwaggerResponse(400, "Requête invalide")]
        [SwaggerResponse(404, "Membre/Livre introuvable")]
        [SwaggerResponse(409, "Livre non disponible")]
        public ActionResult<Emprunt> Create([FromBody] CreateEmpruntRequest body)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = _empruntService.CreateEmpruntAsync(
                    body.MembreId,
                    body.LivreIds,
                    body.DateRetourPrevue,
                    body.EmployeValidationId);

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return Conflict(ex.Message); }
        }

        // PUT: api/emprunts/{id}/retour/{livreId}
        [HttpPut("{id:int}/retour/{livreId:int}")]
        [SwaggerOperation(Summary = "Retourne un livre spécifique d’un emprunt")]
        [SwaggerResponse(200, "OK", typeof(Emprunt))]
        [SwaggerResponse(404, "Emprunt ou livre non trouvé")]
        public async Task<ActionResult<Emprunt>> RetourLivre(int id, int livreId, [FromQuery] int employeValidationId)
        {
            try
            {
                var updated = _empruntService.ReturnSpecificBookFromEmpruntAsync(id, livreId, employeValidationId);
                return Ok(updated);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }
        /*
        // PUT: api/emprunts/{id}/retour
        [HttpPut("{id:int}/retour")]
        [SwaggerOperation(Summary = "Retourne tous les livres d’un emprunt (clôture)")]
        [SwaggerResponse(200, "OK", typeof(Emprunt))]
        [SwaggerResponse(404, "Emprunt non trouvé")]
        public async Task<ActionResult<Emprunt>> RetourTout(int id, [FromQuery] int employeValidationId)
        {
            try
            {
                var updated = _empruntService.Re(id, employeValidationId);
                return Ok(updated);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }
*/
        // DELETE: api/emprunts/{id}
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Supprime un emprunt")]
        [SwaggerResponse(204, "Supprimé")]
        [SwaggerResponse(404, "Emprunt non trouvé")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = _empruntService.DeleteEmpruntAsync(id);
                if (deleted == null) return NotFound();
                return NoContent();
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }
    }
}

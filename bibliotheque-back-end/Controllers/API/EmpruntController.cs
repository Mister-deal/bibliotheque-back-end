using System.ComponentModel.DataAnnotations;
using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.DTO;
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
        private readonly IStatistiquesService _statistiquesService;

        public EmpruntsController(IEmpruntService empruntService, IStatistiquesService statistiquesService)
        {
            _empruntService = empruntService;
            _statistiquesService = statistiquesService;
        }

        public class CreateEmpruntRequest
        {
            [Required] public int MembreId { get; set; }

            [Required, MinLength(1)]
            public List<int> LivreIds { get; set; } = new();

            public DateOnly? DateRetourPrevue { get; set; }

            [Required] public int EmployeValidationId { get; set; }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Liste tous les emprunts")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Emprunt>))]
        public async Task<ActionResult<IEnumerable<Emprunt>>> GetAll()
            => Ok(await _empruntService.GetAllEmpruntsAsync());

        [HttpGet("actifs")]
        [SwaggerOperation(Summary = "Liste les emprunts actifs (non clôturés)")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Emprunt>))]
        public async Task<ActionResult<IEnumerable<Emprunt>>> GetActifs()
        {
            var activeEmprunts = await _empruntService.GetActiveEmpruntsAsync();
            return Ok(activeEmprunts);
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Récupère un emprunt par son Id")]
        [SwaggerResponse(200, "OK", typeof(Emprunt))]
        [SwaggerResponse(404, "Emprunt non trouvé")]
        public async Task<ActionResult<Emprunt>> GetById(int id)
        {
            try
            {
                var e = await _empruntService.GetEmpruntByIdAsync(id);
                return Ok(e);
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("membre/{membreId:int}")]
        [SwaggerOperation(Summary = "Récupère les emprunts d'un membre")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Emprunt>))]
        public async Task<ActionResult<IEnumerable<Emprunt>>> GetByMembre(int membreId)
        {
            try
            {
                return Ok(await _empruntService.GetEmpruntsByMembreIdAsync(membreId));
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("livre/{livreId:int}")]
        [SwaggerOperation(Summary = "Récupère les emprunts contenant un livre donné")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Emprunt>))]
        public async Task<ActionResult<IEnumerable<Emprunt>>> GetByLivre(int livreId)
        {
            try
            {
                return Ok(await _empruntService.GetEmpruntsByLivreIdAsync(livreId));
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Crée un nouvel emprunt",
            Description = "Crée un emprunt pour un membre avec une liste de livres. Les livres passent en état 'Emprunte'.")]
        [SwaggerResponse(201, "Créé", typeof(Emprunt))]
        [SwaggerResponse(400, "Requête invalide")]
        [SwaggerResponse(404, "Membre/Livre introuvable")]
        [SwaggerResponse(409, "Livre non disponible")]
        [HttpPost]
        public async Task<ActionResult<Emprunt>> CreateEmprunt([FromBody] EmpruntCreateDto empruntCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            try
            {
                var newEmprunt = await _empruntService.CreateEmpruntAsync(empruntCreateDto);

                return CreatedAtAction(nameof(GetEmpruntsEnCours), new { id = newEmprunt.Id }, newEmprunt);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // Pour les cas comme "livre non disponible"
            }
            catch (Exception ex)
            {
                // Gestion générique des erreurs imprévues
                return StatusCode(500, $"Erreur interne du serveur : {ex.Message}");
            }
        }

        [HttpPut("{id:int}/retour/{livreId:int}")]
        [SwaggerOperation(Summary = "Retourne un livre spécifique d'un emprunt")]
        [SwaggerResponse(200, "OK", typeof(Emprunt))]
        [SwaggerResponse(404, "Emprunt ou livre non trouvé")]
        public async Task<ActionResult<Emprunt>> RetourLivre(int id, int livreId, [FromQuery] int employeValidationId)
        {
            try
            {
                var updated = await _empruntService.ReturnSpecificBookFromEmpruntAsync(id, livreId, employeValidationId);
                return Ok(updated);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpPut("{id:int}/retour")]
        [SwaggerOperation(Summary = "Retourne tous les livres d'un emprunt (clôture)")]
        [SwaggerResponse(200, "OK", typeof(Emprunt))]
        [SwaggerResponse(404, "Emprunt non trouvé")]
        public async Task<ActionResult<Emprunt>> RetourTout(int id, [FromQuery] int employeValidationId)
        {
            try
            {
                var updated = await _empruntService.ReturnAllBooksForEmpruntAsync(id, employeValidationId);
                return Ok(updated);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Supprime un emprunt")]
        [SwaggerResponse(204, "Supprimé")]
        [SwaggerResponse(404, "Emprunt non trouvé")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _empruntService.DeleteEmpruntAsync(id);
                if (deleted == null) return NotFound();
                return NoContent();
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("emprunts-en-cours")]
        [SwaggerOperation(Summary = "Nombre d'emprunts en cours")]
        public async Task<ActionResult<int>> GetEmpruntsEnCours()
        {
            var count = await _statistiquesService.GetNombreEmpruntsEnCoursAsync();
            return Ok(count);
        }

        [HttpGet("emprunts-en-retard")]
        [SwaggerOperation(Summary = "Nombre d'emprunts en retard")]
        public async Task<ActionResult<int>> GetEmpruntsEnRetard()
        {
            var count = await _statistiquesService.GetNombreEmpruntsEnRetardAsync();
            return Ok(count);
        }

        [HttpGet("top5-auteurs")]
        [SwaggerOperation(Summary = "Top 5 des auteurs les plus empruntés")]
        public async Task<ActionResult<List<object>>> GetTop5Auteurs()
        {
            var top5 = await _statistiquesService.GetTop5AuteursPopulairesAsync();
            return Ok(top5);
        }
    }
}

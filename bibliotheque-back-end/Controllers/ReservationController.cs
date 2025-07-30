using bibliotheque_back_end.Models;
using bibliotheque_back_end.Models.DTO;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Controllers;
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Contrôleur de gestion des reservations de la bibliothèque")]
public class ReservationController: ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    //testé
    //get : api/reservations
    [HttpGet]
    [SwaggerOperation(
        Summary = "Récupère toutes les reservations",
        Description = "Retourne la liste complète des réservations enregistrées en base de données"
    )]
    [SwaggerResponse(404, "reservations non trouvées")]
    [SwaggerResponse(400, "Requête invalide (ID négatif ou zéro)")] // Ajout du 400
    [SwaggerResponse(200, "Liste des reservations retournée avec succès", typeof(IEnumerable<Reservation>))] 
    public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
    {
        var reservations = await _reservationService.GetAllReservationsAsync();
        return Ok(reservations);
    }
    
    //get : api/reservations/{id}
    //testé
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Récupère une reservation par son identifiant",
        Description = "Permet de récupérer une réservation spécifique via son identifiant unique"
    )]
    [SwaggerResponse(200, "reservation trouvée", typeof(Livre))]
    [SwaggerResponse(404, "reservation non trouvée")]
    [SwaggerResponse(400, "Requête invalide (ID négatif ou zéro)")] // Ajout du 400
    public async Task<ActionResult<Reservation>> GetReservationById(int id)
    {
        try
        {
            var reservation = await _reservationService.GetReservationAsync(id);
            if (reservation is null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    //get : api/reservations/actif
    //a tester
    [HttpGet("actif")]
    [SwaggerOperation(
        Summary = "Récupère les reservations actives",
        Description = "Permet de récupérer les reservations actives"
    )]
    [SwaggerResponse(200, "reservations trouvées", typeof(Livre))]
    [SwaggerResponse(404, "reservations non trouvées")]
    [SwaggerResponse(400, "Requête invalide (ID négatif ou zéro)")] // Ajout du 400
        public async Task<ActionResult<IEnumerable<Reservation>>> GetActiveReservations()
        {
            var activeReservations = await _reservationService.GetActiveReservationsAsync();
            return Ok(activeReservations);
        }

        //get : api/reservations/membre/{id}
        //a tester
        [HttpGet("membre/{memberId}")]
        [SwaggerOperation(
            Summary = "Récupère une reservation par l'identifiant de son membre",
            Description = "Permet de récupérer une réservation spécifique via l'identifiant de son membre"
        )]
        [SwaggerResponse(200, "reservation trouvée", typeof(Livre))]
        [SwaggerResponse(404, "reservation non trouvée")]
        [SwaggerResponse(400, "Requête invalide (ID négatif ou zéro)")] // Ajout du 400
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationByMemberId(int memberId)
        {
            try
            {
                var reservationMemberId = await _reservationService.GetReservationsByMembreIdAsync(memberId);
                if (reservationMemberId == null || !reservationMemberId.Any())
                {
                    return NotFound();
                }
                return Ok(reservationMemberId);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        //get : api/reservations/livre/{id}
        //a tester
        [HttpGet("livre/{livreId}")]
        [SwaggerOperation(
            Summary = "Récupère une reservation par l'identifiant de livre",
            Description = "Permet de récupérer une réservation spécifique via l'identifiant de livre"
        )]
        [SwaggerResponse(200, "reservation trouvée", typeof(Livre))]
        [SwaggerResponse(404, "reservation non trouvée")]
        [SwaggerResponse(400, "Requête invalide (ID négatif ou zéro)")] // Ajout du 400
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationByLivreId(int livreId)
        {
            try
            {
                var reservationLivreId = await _reservationService.GetReservationsByLivreIdAsync(livreId);
                if (reservationLivreId == null || !reservationLivreId.Any())
                {
                    return NotFound();
                }
                return Ok(reservationLivreId);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    
}
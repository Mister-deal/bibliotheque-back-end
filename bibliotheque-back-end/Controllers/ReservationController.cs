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
    
    public async Task<ActionResult<IEnumerable<Reservation>>> GetActiveReservationById()
    {
        var activeReservations = await _reservationService.GetActiveReservationsAsync();
        return Ok(activeReservations);
    }

    public async Task<ActionResult<Reservation>> GetReservationByMemberId(int memberId)
    {
        try
        {
            var reservationMemberId = await _reservationService.GetReservationsByMembreIdAsync(memberId);
            if (reservationMemberId == null)
            {
                return NotFound();
            }
            return Ok(reservationMemberId);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    public async Task<ActionResult<Reservation>> GetReservationByLivreId(int livreId)
    {
        try
        {
            var reservationLivreId = await _reservationService.GetReservationsByLivreIdAsync(livreId);
            if (reservationLivreId == null)
            {
                return NotFound();
            }
            return Ok(reservationLivreId);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}
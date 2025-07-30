using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ILivreRepository _livreRepository;
    private readonly IMembreRepository _membreRepository;
    private readonly BibliothequeDb _context;

    public ReservationService(
        IReservationRepository reservationRepository,
        ILivreRepository livreRepository,
        IMembreRepository membreRepository,
        BibliothequeDb context)
    {
        _reservationRepository = reservationRepository;
        _livreRepository = livreRepository;
        _membreRepository = membreRepository;
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
    {
        return await _reservationRepository.GetAllReservationsAsync();
    }

    public async Task<Reservation?> GetReservationAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID de la réservation doit être positif.", nameof(id));
        }

        var reservation = await _reservationRepository.GetReservationAsync(id);
        return reservation;
    }

    public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync()
    {
        var allReservations = await _reservationRepository.GetAllReservationsAsync();
        return allReservations.Where(r => r.EstActive);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByMembreIdAsync(int membreId)
    {
        if (membreId <= 0)
        {
            throw new ArgumentException("L'ID du membre doit être positif.", nameof(membreId));
        }
        var allReservations = await _reservationRepository.GetAllReservationsAsync();
        return allReservations.Where(r => r.MembreId == membreId);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByLivreIdAsync(int livreId)
    {
        if (livreId <= 0)
        {
            throw new ArgumentException("L'ID du livre doit être positif.", nameof(livreId));
        }
        var allReservations = await _reservationRepository.GetAllReservationsAsync();
        return allReservations.Where(r => r.LivreId == livreId);
    }

    public async Task<bool> HasActiveReservationAsync(int membreId, int livreId)
    {
        if (membreId <= 0 || livreId <= 0)
        {
            throw new ArgumentException("Les IDs du membre et du livre doivent être positifs.");
        }
        var reservations = await GetReservationsByMembreIdAsync(membreId);
        return reservations.Any(r => r.LivreId == livreId && r.EstActive);
    }

    public async Task<Reservation> CreateReservationAsync(int membreId, int livreId)
    {
        if (membreId <= 0 || livreId <= 0)
        {
            throw new ArgumentException("Les IDs du membre et du livre doivent être positifs.");
        }

        var membre = await _membreRepository.GetMemberAsync(membreId);
        if (membre == null)
        {
            throw new KeyNotFoundException($"Le membre avec l'ID {membreId} n'existe pas.");
        }

        var livre = await _livreRepository.GetBookByIdAsync(livreId);
        if (livre == null)
        {
            throw new KeyNotFoundException($"Le livre avec l'ID {livreId} n'existe pas.");
        }
        
        if (await HasActiveReservationAsync(membreId, livreId))
        {
            throw new InvalidOperationException($"Le membre a déjà une réservation active pour le livre '{livre.Titre}'.");
        }

        var newReservation = new Reservation
        {
            MembreId = membreId,
            LivreId = livreId,
            DateReservation = DateOnly.FromDateTime(DateTime.Now),
            EstActive = true
        };

        await _reservationRepository.CreateReservationAsync(newReservation);
        await _context.SaveChangesAsync();

        return newReservation;
    }

    public async Task<Reservation?> FulfillReservationAsync(int reservationId)
    {
        if (reservationId <= 0)
        {
            throw new ArgumentException("L'ID de la réservation doit être positif.", nameof(reservationId));
        }

        var reservation = await _reservationRepository.GetReservationAsync(reservationId);
        if (reservation == null)
        {
            return null; // Réservation non trouvée
        }
        
        if (!reservation.EstActive)
        {
            throw new InvalidOperationException($"La réservation {reservationId} ne peut pas être honorée car elle n'est pas active.");
        }

        reservation.EstActive = false; 

        await _reservationRepository.UpdateReservationAsync(reservation);
        await _context.SaveChangesAsync();
        

        return reservation;
    }

    public async Task<Reservation?> CancelReservationAsync(int reservationId)
    {
        if (reservationId <= 0)
        {
            throw new ArgumentException("L'ID de la réservation doit être positif.", nameof(reservationId));
        }

        var reservation = await _reservationRepository.GetReservationAsync(reservationId);
        if (reservation == null)
        {
            return null; // Réservation non trouvée
        }

        // Une réservation peut être annulée si elle est active.
        if (!reservation.EstActive)
        {
            throw new InvalidOperationException($"La réservation {reservationId} ne peut pas être annulée car elle n'est pas active.");
        }

        reservation.EstActive = false;

        await _reservationRepository.UpdateReservationAsync(reservation);
        await _context.SaveChangesAsync();

        return reservation;
    }

    public async Task<Reservation?> DeleteReservationAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID de la réservation doit être positif.", nameof(id));
        }

        var existingReservation = await _reservationRepository.GetReservationAsync(id);
        if (existingReservation == null)
        {
            return null; // Réservation non trouvée
        }

        if (existingReservation.EstActive)
        {
            throw new InvalidOperationException($"Impossible de supprimer la réservation {id} car elle est toujours active. Annulez-la d'abord.");
        }

        await _reservationRepository.DeleteReservationAsync(existingReservation);
        await _context.SaveChangesAsync();

        return existingReservation;
    }

    public async Task<bool> ReservationExistsAsync(int id)
    {
        return await _reservationRepository.CheckIfReservationExistsAsync(id);
    }
}
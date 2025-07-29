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
        // Une réservation est "active" si EstActive est vraie.
        return allReservations.Where(r => r.EstActive);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByMembreIdAsync(int membreId)
    {
        if (membreId <= 0)
        {
            throw new ArgumentException("L'ID du membre doit être positif.", nameof(membreId));
        }
        
        // Idéalement, _reservationRepository.GetReservationsByMembreIdAsync(membreId)
        var allReservations = await _reservationRepository.GetAllReservationsAsync();
        return allReservations.Where(r => r.MembreId == membreId);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsByLivreIdAsync(int livreId)
    {
        if (livreId <= 0)
        {
            throw new ArgumentException("L'ID du livre doit être positif.", nameof(livreId));
        }
        
        // Idéalement, _reservationRepository.GetReservationsByLivreIdAsync(livreId)
        var allReservations = await _reservationRepository.GetAllReservationsAsync();
        return allReservations.Where(r => r.LivreId == livreId);
    }

    public async Task<bool> HasActiveReservationAsync(int membreId, int livreId)
    {
        if (membreId <= 0 || livreId <= 0)
        {
            throw new ArgumentException("Les IDs du membre et du livre doivent être positifs.");
        }
        
        // Idéalement, _reservationRepository.HasActiveReservationAsync(membreId, livreId)
        var reservations = await GetReservationsByMembreIdAsync(membreId);
        // Vérifie si le membre a une réservation active pour ce livre
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

        // Logique métier :
        // 1. Vérifier si le livre est disponible pour la réservation (par exemple, EtatLivre.Disponible)
        // Vous devrez implémenter cette vérification dans votre LivreService ou ici.
        // if (livre.Etat != EtatLivre.Disponible)
        // {
        //     throw new InvalidOperationException($"Le livre '{livre.Titre}' n'est pas disponible pour la réservation.");
        // }

        // 2. Vérifier si le membre n'a pas déjà une réservation active pour ce livre
        if (await HasActiveReservationAsync(membreId, livreId))
        {
            throw new InvalidOperationException($"Le membre a déjà une réservation active pour le livre '{livre.Titre}'.");
        }

        var newReservation = new Reservation
        {
            MembreId = membreId,
            LivreId = livreId,
            DateReservation = DateOnly.FromDateTime(DateTime.Now),
            EstActive = true // La nouvelle réservation est active par défaut
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

        // Une réservation peut être honorée si elle est active.
        if (!reservation.EstActive)
        {
            throw new InvalidOperationException($"La réservation {reservationId} ne peut pas être honorée car elle n'est pas active.");
        }
        
        // Logique métier additionnelle (ex: vérifier disponibilité du livre)
        // var livre = await _livreRepository.GetBookByIdAsync(reservation.LivreId);
        // if (livre?.Etat != EtatLivre.Disponible)
        // {
        //     throw new InvalidOperationException($"Le livre '{livre?.Titre}' n'est plus disponible pour honorer la réservation.");
        // }

        reservation.EstActive = false; // La réservation n'est plus active
        // Vous pourriez ajouter une propriété DateHonoree si vous voulez garder une trace du moment où elle a été honorée
        // reservation.DateHonoree = DateOnly.FromDateTime(DateTime.Now);

        await _reservationRepository.UpdateReservationAsync(reservation);
        await _context.SaveChangesAsync();

        // Optionnel: Mettre à jour l'état du livre (par exemple, le marquer comme emprunté) si l'honneur de la réservation entraîne un emprunt
        // if (livre != null) {
        //     livre.Etat = EtatLivre.Emprunte;
        //     await _livreRepository.UpdateBookAsync(livre);
        //     await _context.SaveChangesAsync();
        // }

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

        reservation.EstActive = false; // La réservation n'est plus active
        // Vous pourriez ajouter une propriété DateAnnulee si vous voulez garder une trace du moment de l'annulation
        // reservation.DateAnnulee = DateOnly.FromDateTime(DateTime.Now);

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

        // Logique métier : Si une réservation est toujours active, on pourrait empêcher sa suppression directe
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
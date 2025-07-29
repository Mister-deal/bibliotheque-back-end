using bibliotheque_back_end.Data;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque_back_end.Models.repositery;

public class ReservationRepository : IReservationRepository
{
    private readonly BibliothequeDb _context;

    public ReservationRepository(BibliothequeDb context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
    {
        // Utilise ToListAsync() pour récupérer toutes les réservations de manière asynchrone,
        // en incluant les entités Membre et Livre associées.
        return await _context.Reservations
                             .Include(r => r.Membre)
                             .Include(r => r.Livre)
                             .ToListAsync();
    }

    public async Task<Reservation?> GetReservationAsync(int id)
    {
        // Utilise FirstOrDefaultAsync() pour récupérer une réservation spécifique de manière asynchrone,
        // en incluant les entités associées.
        return await _context.Reservations
                             .Include(r => r.Membre)
                             .Include(r => r.Livre)
                             .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task CreateReservationAsync(Reservation reservation)
    {
        // Utilise AddAsync() pour ajouter l'entité de manière asynchrone.
        await _context.Reservations.AddAsync(reservation);
        // Rappel : les modifications devront être sauvegardées via _context.SaveChangesAsync() depuis la couche de service.
    }

    public Task UpdateReservationAsync(Reservation reservation)
    {
        // Update() est synchrone car il ne fait que modifier l'état de l'entité en mémoire.
        // La persistance réelle se produit avec SaveChangesAsync() (à appeler depuis le service).
        _context.Reservations.Update(reservation);
        return Task.CompletedTask; // Retourne un Task complété.
    }

    public Task DeleteReservationAsync(Reservation reservation)
    {
        // Remove() est synchrone.
        _context.Reservations.Remove(reservation);
        return Task.CompletedTask; // Retourne un Task complété.
    }

    public async Task<bool> CheckIfReservationExistsAsync(int id)
    {
        // Utilise AnyAsync() pour vérifier l'existence de manière asynchrone.
        return await _context.Reservations.AnyAsync(e => e.Id == id);
    }
}
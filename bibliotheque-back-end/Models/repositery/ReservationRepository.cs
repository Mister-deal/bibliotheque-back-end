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
        //ToListAsync() pour récupérer toutes les réservations de manière asynchrone
        return await _context.Reservations
                             .Include(r => r.Membre)
                             .Include(r => r.Livre)
                             .ToListAsync();
    }

    public async Task<Reservation?> GetReservationAsync(int id)
    {
        //FirstOrDefaultAsync() pour récupérer une réservation spécifique de manière asynchrone
        return await _context.Reservations
                             .Include(r => r.Membre)
                             .Include(r => r.Livre)
                             .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task CreateReservationAsync(Reservation reservation)
    {
        //AddAsync() pour ajouter l'entité de manière asynchrone.
        await _context.Reservations.AddAsync(reservation);
    }

    public Task UpdateReservationAsync(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        return Task.CompletedTask;
    }

    public Task DeleteReservationAsync(Reservation reservation)
    {
        _context.Reservations.Remove(reservation);
        return Task.CompletedTask;
    }

    public async Task<bool> CheckIfReservationExistsAsync(int id)
    {
        //AnyAsync() pour vérifier l'existence de manière asynchrone.
        return await _context.Reservations.AnyAsync(e => e.Id == id);
    }
}
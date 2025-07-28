using bibliotheque_back_end.Data;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque_back_end.Models.repositery;

public class ReservationRepository: IReservationRepository
{
    private readonly BibliothequeDb _context;

    public ReservationRepository(BibliothequeDb context)
    {
        _context = context;
    }

    public IEnumerable<Reservation> GetAllReservations()
    {
        return _context.Reservations.Include(r => r.Membre).Include(r => r.Livre).ToList();
    }

    public Reservation GetReservation(int id)
    {
        return _context.Reservations.Include(r => r.Membre).Include(r => r.Livre).FirstOrDefault(r => r.Id == id);
    }

    public void CreateReservation(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
    }

    public void UpdateReservation(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
    }

    public void DeleteReservation(Reservation reservation)
    {
        _context.Reservations.Remove(reservation);
    }

    public bool CheckIfReservationExists(int id)
    {
        return _context.Reservations.Any(e => e.Id == id);
    }
}
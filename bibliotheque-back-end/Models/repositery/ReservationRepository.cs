using bibliotheque_back_end.Data;

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
        throw new NotImplementedException();
    }

    public Reservation GetReservation(int id)
    {
        throw new NotImplementedException();
    }

    public void CreateReservation(Reservation reservation)
    {
        throw new NotImplementedException();
    }

    public void UpdateReservation(Reservation reservation)
    {
        throw new NotImplementedException();
    }

    public void DeleteReservation(Reservation reservation)
    {
        throw new NotImplementedException();
    }

    public bool checkIfReservationExists(int id)
    {
        throw new NotImplementedException();
    }
}
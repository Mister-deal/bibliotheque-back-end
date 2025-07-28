using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class ReservationService: IReservationService
{
    public IEnumerable<Reservation> GetAllReservations()
    {
        throw new NotImplementedException();
    }

    public Reservation GetReservation(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Reservation> GetActiveReservations()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Reservation> GetReservationsByMembreId(int membreId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Reservation> GetReservationsByLivreId(int livreId)
    {
        throw new NotImplementedException();
    }

    public bool HasActiveReservation(int membreId, int livreId)
    {
        throw new NotImplementedException();
    }

    public Reservation CreateReservation(int membreId, int livreId)
    {
        throw new NotImplementedException();
    }

    public Reservation FulfillReservation(int reservationId)
    {
        throw new NotImplementedException();
    }

    public Reservation CancelReservation(int reservationId)
    {
        throw new NotImplementedException();
    }

    public Reservation DeleteReservation(int id)
    {
        throw new NotImplementedException();
    }

    public bool ReservationExists(int id)
    {
        throw new NotImplementedException();
    }
}
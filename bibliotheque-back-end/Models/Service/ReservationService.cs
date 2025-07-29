using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class ReservationService: IReservationService
{
    private readonly IReservationRepository _reservationRepository;

    public ReservationService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public IEnumerable<Reservation> GetAllReservations()
    {
        return  _reservationRepository.GetAllReservations();
    }

    public Reservation GetReservation(int id)
    {
        if (id <= 0) throw new ArgumentException("Book ID doit être positif", nameof(id));
        var reservation = _reservationRepository.GetReservation(id);
        if (id == null) throw new ArgumentException($"livre avec l'id {id} non trouvé");
        return reservation;
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
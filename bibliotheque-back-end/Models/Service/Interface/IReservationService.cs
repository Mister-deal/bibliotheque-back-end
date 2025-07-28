namespace bibliotheque_back_end.Models.Service.Interface;

public interface IReservationService
{
    IEnumerable<Reservation> GetAllReservations();
    Reservation GetReservation(int id);
    
    IEnumerable<Reservation> GetActiveReservations(); // Réservations non encore honorées ou annulées
    IEnumerable<Reservation> GetReservationsByMembreId(int membreId);
    IEnumerable<Reservation> GetReservationsByLivreId(int livreId);
    
    bool HasActiveReservation(int membreId, int livreId);
    
    Reservation CreateReservation(int membreId, int livreId);
    Reservation FulfillReservation(int reservationId);

    Reservation CancelReservation(int reservationId);
    Reservation DeleteReservation(int id);
    
    bool ReservationExists(int id);

}
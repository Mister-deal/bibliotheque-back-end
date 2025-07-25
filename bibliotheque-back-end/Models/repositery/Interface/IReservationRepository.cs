namespace bibliotheque_back_end.Models.repositery;

public interface IReservationRepository
{
    IEnumerable<Reservation> GetAllReservations();
    Reservation GetReservation(int id);
    void CreateReservation(Reservation reservation);
    void UpdateReservation(Reservation reservation);
    void DeleteReservation(Reservation reservation);
    bool checkIfReservationExists(int id);
}
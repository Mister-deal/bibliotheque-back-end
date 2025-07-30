namespace bibliotheque_back_end.Models.Service.Interface;

public interface IReservationService
{
    Task<IEnumerable<Reservation>> GetAllReservationsAsync();
    Task<Reservation?> GetReservationAsync(int id); // Peut retourner null si non trouvée
    
    Task<IEnumerable<Reservation>> GetActiveReservationsAsync(); // Réservations non encore honorées ou annulées
    Task<IEnumerable<Reservation>> GetReservationsByMembreIdAsync(int membreId);
    Task<IEnumerable<Reservation>> GetReservationsByLivreIdAsync(int livreId);
    
    Task<bool> HasActiveReservationAsync(int membreId, int livreId);
    
    Task<Reservation> CreateReservationAsync(int membreId, int livreId);
    Task<Reservation?> FulfillReservationAsync(int reservationId); // Peut retourner null si non trouvée ou échec
    Task<Reservation?> CancelReservationAsync(int reservationId); // Peut retourner null si non trouvée ou échec
    Task<Reservation?> DeleteReservationAsync(int id); // Peut retourner null si non trouvée ou échec
    
    Task<bool> ReservationExistsAsync(int id);
}
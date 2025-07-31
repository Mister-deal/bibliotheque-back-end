namespace bibliotheque_back_end.Models.Service.Interface;

public interface IEmpruntService
{
    Task<IEnumerable<Emprunt>> GetAllEmpruntsAsync();
    Task<IEnumerable<Emprunt>> GetEmpruntsByMembreIdAsync(int membreId);
    Task<IEnumerable<Emprunt>> GetEmpruntsByLivreIdAsync(int livreId);
    Task<Emprunt?> GetEmpruntByIdAsync(int id); // Peut retourner null si non trouvé
    Task<IEnumerable<Emprunt>> GetActiveEmpruntsAsync();
    
    Task<Emprunt> CreateEmpruntAsync(int membreId, List<int> livreIds, DateOnly? dateRetour, int employeValidationId);
    Task<Emprunt?> ReturnSpecificBookFromEmpruntAsync(int empruntId, int livreId, int employeValidationId); // Peut retourner null si non trouvé ou si l'opération échoue
    Task<Emprunt?> ReturnAllBooksForEmpruntAsync(int empruntId, int employeValidationId);
    Task<Emprunt?> DeleteEmpruntAsync(int id); // Peut retourner null si non trouvé
    Task<bool> EmpruntExistsAsync(int id);

    // PArtie Dashboard
    Task<int> GetTodayLoansCountAsync();
    Task<int> GetOverdueReturnsCountAsync();
    Task<IEnumerable<dynamic>> GetRecentActivitiesAsync();
    Task<IEnumerable<dynamic>> GetPopularBooksAsync();
}
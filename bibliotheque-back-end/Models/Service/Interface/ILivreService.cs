namespace bibliotheque_back_end.Models.Service.Interface;

public interface ILivreService
{
    Task<IEnumerable<Livre>> GetAllBooksAsync();
    Task<Livre?> GetBookByIdAsync(int id); // Peut retourner null si non trouvé
    Task<Livre> AddNewBookAsync(Livre newBook);
    Task<Livre?> UpdateBookAsync(int id, Livre updatedBook); // Peut retourner null si non trouvé
    Task DeleteBookAsync(int id); // Retourne un Task car l'opération est asynchrone
    Task<IEnumerable<Livre>> GetAvailableBooksAsync();
}
namespace bibliotheque_back_end.Models.repositery;

public interface ILivreRepository
{
    Task<IEnumerable<Livre>> GetAllBooksAsync();
    Task<Livre?> GetBookByIdAsync(int id);
    Task CreateBookAsync(Livre livre);
    Task UpdateBookAsync(Livre livre);
    Task DeleteBookAsync(Livre livre);
    Task<bool> BookExistsAsync(int id);
}
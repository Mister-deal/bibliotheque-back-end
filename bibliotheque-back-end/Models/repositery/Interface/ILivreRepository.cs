namespace bibliotheque_back_end.Models.repositery;

public interface ILivreRepository
{
    IEnumerable<Livre> GetAllBooks();
    Livre GetBookById(int id);
    void CreateBook(Livre livre);
    void UpdateBook(Livre livre);
    void DeleteBook(Livre livre);
    bool BookExists(int id);
}
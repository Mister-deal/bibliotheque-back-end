namespace bibliotheque_back_end.Models.repositery;

public interface ILivreRepository
{
    IEnumerable<Livre> GetAllBooks();
    Livre GetBookById(int id);
    Livre CreateBook(Livre livre);
    Livre UpdateBook(Livre livre);
    Livre DeleteBook(Livre livre);
    bool BookExists(int id);
}
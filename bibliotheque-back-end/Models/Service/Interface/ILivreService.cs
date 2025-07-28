namespace bibliotheque_back_end.Models.Service.Interface;

public interface ILivreService
{
    IEnumerable<Livre> GetAllBooks();
    Livre GetBookById(int id);
    Livre AddNewBook(Livre newBook);
    Livre UpdateBook(int id, Livre updatedBook);
    void  DeleteBook(int id);
    IEnumerable<Livre> GetAvailableBooks();
}
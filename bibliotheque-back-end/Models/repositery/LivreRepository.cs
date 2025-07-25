using bibliotheque_back_end.Data;

namespace bibliotheque_back_end.Models.repositery;

public class LivreRepository:  ILivreRepository
{
    private readonly BibliothequeDb _context;

    public LivreRepository(BibliothequeDb context)
    {
        _context = context;
    }

    public IEnumerable<Livre> GetAllBooks()
    {
        throw new NotImplementedException();
    }

    public Livre GetBookById(int id)
    {
        throw new NotImplementedException();
    }

    public Livre CreateBook(Livre livre)
    {
        throw new NotImplementedException();
    }

    public Livre UpdateBook(Livre livre)
    {
        throw new NotImplementedException();
    }

    public Livre DeleteBook(Livre livre)
    {
        throw new NotImplementedException();
    }

    public bool BookExists(int id)
    {
        throw new NotImplementedException();
    }
}
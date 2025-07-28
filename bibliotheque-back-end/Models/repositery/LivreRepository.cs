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
        return _context.Livres.ToList();
    }

    public Livre GetBookById(int id)
    {
        return _context.Livres.Find(id);
    }

    public void CreateBook(Livre livre)
    {
        _context.Livres.Add(livre);
    }

    public void UpdateBook(Livre livre)
    {
        _context.Livres.Update(livre);
    }

    public void DeleteBook(Livre livre)
    {
        _context.Livres.Remove(livre);
    }


    public bool BookExists(int id)
    {
        return _context.Livres.Any(e => e.Id == id);
    }
}
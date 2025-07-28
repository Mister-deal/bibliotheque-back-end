using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class LivreService: ILivreService
{
    private readonly IEmpruntRepository _empruntRepository;
    private readonly ILivreRepository _livreRepository;

    public LivreService(IEmpruntRepository empruntRepository, ILivreRepository livreRepository)
    {
        _empruntRepository = empruntRepository;
        _livreRepository = livreRepository;
    }

    public IEnumerable<Livre> GetAllBooks()
    {
        return _livreRepository.GetAllBooks();
    }

    public Livre GetBookById(int id)
    {
        if (id <= 0) throw new ArgumentException("Book ID doit être positif", nameof(id));
        var book = _livreRepository.GetBookById(id);
        if (id == null) throw new ArgumentException($"livre avec l'id {id} non trouvé");
        return book;
    }

    public Livre AddNewBook(Livre newBook)
    {
        if(newBook == null) throw new ArgumentException("Livre doit <UNK>tre vide", nameof(newBook));
        if (string.IsNullOrWhiteSpace(newBook.Titre) || string.IsNullOrWhiteSpace(newBook.Auteur) ||
            string.IsNullOrWhiteSpace(newBook.Editeur))
        {
            throw new ArgumentException("le titre, l'éditeur et le nom de l'auteur sont nécessaire pour un nouveau livre", nameof(newBook));
        }

        newBook.Etat = EtatLivre.Disponible;
        _livreRepository.CreateBook(newBook);
        
        return newBook;
    }

    public Livre UpdateBook(int id, Livre updatedBook)
    {
        if(updatedBook == null) throw new ArgumentException("les données du livre ne peuvent être nulles", nameof(updatedBook));
        if(id != updatedBook.Id) throw new ArgumentException("l'id ne correspond pas à l'id du livre fourni", nameof(id));
        
        var existingBook = _livreRepository.GetBookById(id);
        if(existingBook == null) throw new KeyNotFoundException($"le livre avec l'id {id} non trouvé");
        
        existingBook.Titre = updatedBook.Titre;
        existingBook.Auteur = updatedBook.Auteur;
        existingBook.Description = updatedBook.Description;
        existingBook.AnneePublication = updatedBook.AnneePublication;
        existingBook.Editeur = updatedBook.Editeur;
        existingBook.Categorie = updatedBook.Categorie;
        
        _livreRepository.UpdateBook(existingBook);
        return existingBook;
    }

    public void DeleteBook(int id)
    {
        if(id <= 0) throw new ArgumentException("l'id du livre doit être positive", nameof(id));
        var livreADelete = _livreRepository.GetBookById(id);
        if (livreADelete == null)
        {
            throw new KeyNotFoundException($"l'id du livre {id} n'a pas été trouvée");
        }
        if(livreADelete.Etat == EtatLivre.Emprunte) throw new InvalidOperationException("le livre ne peut être supprimé car il est actuellement emprunté.");
        
        _livreRepository.DeleteBook(livreADelete);
    }

    public IEnumerable<Livre> GetAvailableBooks()
    {
        return _livreRepository.GetAllBooks().Where(x => x.Etat == EtatLivre.Disponible);
    }
}
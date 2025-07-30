using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class LivreService : ILivreService
{
    private readonly IEmpruntRepository _empruntRepository;
    private readonly ILivreRepository _livreRepository;
    private readonly BibliothequeDb _context;

    public LivreService(IEmpruntRepository empruntRepository, ILivreRepository livreRepository, BibliothequeDb context)
    {
        _empruntRepository = empruntRepository;
        _livreRepository = livreRepository;
        _context = context;
    }

    public async Task<IEnumerable<Livre>> GetAllBooksAsync()
    {
        return await _livreRepository.GetAllBooksAsync();
    }

    public async Task<Livre?> GetBookByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID du livre doit être positif.", nameof(id));
        }

        var book = await _livreRepository.GetBookByIdAsync(id);
        return book;
    }

    public async Task<Livre> AddNewBookAsync(Livre newBook)
    {
        if (newBook == null)
        {
            throw new ArgumentNullException(nameof(newBook), "Le livre ne peut pas être nul.");
        }
        if (string.IsNullOrWhiteSpace(newBook.Titre) ||
            string.IsNullOrWhiteSpace(newBook.Auteur) ||
            string.IsNullOrWhiteSpace(newBook.Editeur))
        {
            throw new ArgumentException("Le titre, l'auteur et l'éditeur sont nécessaires pour un nouveau livre.", nameof(newBook));
        }
        
        
        newBook.Etat = EtatLivre.Disponible; // S'assure que le livre est disponible à la création
        
        await _livreRepository.CreateBookAsync(newBook);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return newBook;
    }

    public async Task<Livre?> UpdateBookAsync(int id, Livre updatedBook)
    {
        if (updatedBook == null)
        {
            throw new ArgumentNullException(nameof(updatedBook), "Les données du livre mis à jour ne peuvent être nulles.");
        }
        if (id <= 0)
        {
            throw new ArgumentException("L'ID du livre doit être positif.", nameof(id));
        }
        if (id != updatedBook.Id)
        {
            throw new ArgumentException("L'ID de l'URL ne correspond pas à l'ID du livre fourni.", nameof(id));
        }
        
        var existingBook = await _livreRepository.GetBookByIdAsync(id);
        if (existingBook == null)
        {
            return null; // Retourne null si le livre n'est pas trouvé, conformément à l'interface.
        }
        
        // Mettre à jour les propriétés de l'entité existante
        existingBook.Titre = updatedBook.Titre;
        existingBook.Auteur = updatedBook.Auteur;
        existingBook.Description = updatedBook.Description;
        existingBook.AnneePublication = updatedBook.AnneePublication;
        existingBook.Editeur = updatedBook.Editeur;
        existingBook.Categorie = updatedBook.Categorie;

        await _livreRepository.UpdateBookAsync(existingBook);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements

        return existingBook; // Retourne l'entité mise à jour
    }

    public async Task DeleteBookAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("L'ID du livre doit être positif.", nameof(id));
        }
        
        var livreADelete = await _livreRepository.GetBookByIdAsync(id);
        if (livreADelete == null)
        {
            throw new KeyNotFoundException($"Le livre avec l'ID {id} n'a pas été trouvé.");
        }

        if (livreADelete.Etat == EtatLivre.Emprunte)
        {
            throw new InvalidOperationException($"Le livre '{livreADelete.Titre}' ne peut pas être supprimé car il est actuellement emprunté.");
        }
        
        await _livreRepository.DeleteBookAsync(livreADelete);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements
    }

    public async Task<IEnumerable<Livre>> GetAvailableBooksAsync()
    {
        var allBooks = await _livreRepository.GetAllBooksAsync();
        return allBooks.Where(x => x.Etat == EtatLivre.Disponible);
    }

    // Partie Dashboard
    public async Task<Livre> CreateBookAsync(Livre livre)
    {
        if (livre == null)
        {
            throw new ArgumentNullException(nameof(livre), "Le livre ne peut pas être null.");
        }

        // Validation des données
        if (string.IsNullOrWhiteSpace(livre.Titre))
        {
            throw new ArgumentException("Le titre du livre est requis.", nameof(livre));
        }

        if (string.IsNullOrWhiteSpace(livre.Auteur))
        {
            throw new ArgumentException("L'auteur du livre est requis.", nameof(livre));
        }

        try
        {
            _context.Livres.Add(livre);
            await _context.SaveChangesAsync();

            Console.WriteLine($" Livre créé avec ID: {livre.Id}");
            return livre;
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Erreur lors de la création: {ex.Message}");
            throw new InvalidOperationException($"Erreur lors de la création du livre : {ex.Message}", ex);
        }
    }
}
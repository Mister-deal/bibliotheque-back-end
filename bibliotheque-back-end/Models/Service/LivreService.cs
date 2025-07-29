using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class LivreService : ILivreService
{
    private readonly IEmpruntRepository _empruntRepository;
    private readonly ILivreRepository _livreRepository;
    private readonly BibliothequeDb _context; // Ajout du DbContext pour SaveChangesAsync()

    public LivreService(IEmpruntRepository empruntRepository, ILivreRepository livreRepository, BibliothequeDb context)
    {
        _empruntRepository = empruntRepository;
        _livreRepository = livreRepository;
        _context = context; // Initialisation du DbContext
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
        // L'interface permet un retour null, donc pas d'exception KeyNotFoundException ici.
        // C'est à la couche appelante de gérer le cas où le livre n'est pas trouvé.
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

        // Vérifiez l'existence d'un livre avec le même titre, auteur et éditeur pour éviter les doublons.
        // Cela nécessiterait une nouvelle méthode dans ILivreRepository, par exemple :
        // if (await _livreRepository.BookExistsByDetailsAsync(newBook.Titre, newBook.Auteur, newBook.Editeur))
        // {
        //     throw new InvalidOperationException($"Un livre avec le titre '{newBook.Titre}' par '{newBook.Auteur}' existe déjà.");
        // }
        
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
        
        // Note : Le statut (Etat) d'un livre ne doit probablement pas être mis à jour directement via cette méthode
        // pour des raisons de logique métier (par exemple, un livre emprunté ne devient pas "disponible" par une simple mise à jour).
        // Si vous voulez permettre la mise à jour de l'état, ajoutez une validation spécifique.
        // existingBook.Etat = updatedBook.Etat; 

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
            // L'interface renvoie Task, donc on ne peut pas retourner null.
            // On peut lancer une exception spécifique ou simplement ne rien faire si l'élément n'existe pas.
            throw new KeyNotFoundException($"Le livre avec l'ID {id} n'a pas été trouvé.");
        }

        if (livreADelete.Etat == EtatLivre.Emprunte)
        {
            throw new InvalidOperationException($"Le livre '{livreADelete.Titre}' ne peut pas être supprimé car il est actuellement emprunté.");
        }
        
        // Vous pouvez également vérifier les réservations actives pour ce livre si pertinent
        // bool hasActiveReservation = await _empruntRepository.HasActiveReservationForBookAsync(id); // Nécessiterait une méthode dans IEmpruntRepository
        // if (hasActiveReservation) { throw new InvalidOperationException("Le livre ne peut être supprimé car il a des réservations actives."); }

        await _livreRepository.DeleteBookAsync(livreADelete);
        await _context.SaveChangesAsync(); // Sauvegarde asynchrone des changements
    }

    public async Task<IEnumerable<Livre>> GetAvailableBooksAsync()
    {
        var allBooks = await _livreRepository.GetAllBooksAsync();
        return allBooks.Where(x => x.Etat == EtatLivre.Disponible);
    }
}
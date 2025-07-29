using bibliotheque_back_end.Data;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque_back_end.Models.repositery;
public class LivreRepository : ILivreRepository
{
    private readonly BibliothequeDb _context;

    public LivreRepository(BibliothequeDb context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Livre>> GetAllBooksAsync()
    {
        // Utilise ToListAsync() pour récupérer tous les livres de manière asynchrone.
        return await _context.Livres.ToListAsync();
    }

    public async Task<Livre?> GetBookByIdAsync(int id)
    {
        // Utilise FindAsync() pour rechercher un livre par clé primaire de manière asynchrone.
        return await _context.Livres.FindAsync(id);
    }

    public async Task CreateBookAsync(Livre livre)
    {
        // Utilise AddAsync() pour ajouter l'entité de manière asynchrone.
        await _context.Livres.AddAsync(livre);
        // Important: _context.SaveChanges() est retiré ici.
        // Les changements devront être persistés par un appel à _context.SaveChangesAsync()
        // depuis la couche de service ou une unité de travail.
    }

    public Task UpdateBookAsync(Livre livre)
    {
        // Update() est synchrone car il modifie simplement l'état de l'entité dans le DbContext en mémoire.
        // La persistance réelle se fera lors de l'appel asynchrone à SaveChangesAsync() ailleurs.
        _context.Livres.Update(livre);
        return Task.CompletedTask; // Retourne un Task complété car il n'y a pas d'opération asynchrone d'attente ici.
    }

    public Task DeleteBookAsync(Livre livre)
    {
        // Remove() est synchrone, similaire à Update().
        _context.Livres.Remove(livre);
        return Task.CompletedTask; // Retourne un Task complété.
    }

    public async Task<bool> BookExistsAsync(int id)
    {
        // Utilise AnyAsync() pour vérifier l'existence de manière asynchrone.
        return await _context.Livres.AnyAsync(e => e.Id == id);
    }
}
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
        //ToListAsync() pour récupérer tous les livres de manière asynchrone.
        return await _context.Livres.ToListAsync();
    }

    public async Task<Livre?> GetBookByIdAsync(int id)
    {
        //FindAsync() pour rechercher un livre par clé primaire de manière asynchrone.
        return await _context.Livres.FindAsync(id);
    }

    public async Task CreateBookAsync(Livre livre)
    {
        //AddAsync() pour ajouter l'entité de manière asynchrone.
        await _context.Livres.AddAsync(livre);
    }

    public Task UpdateBookAsync(Livre livre)
    {
        
        _context.Livres.Update(livre);
        return Task.CompletedTask;
    }

    public Task DeleteBookAsync(Livre livre)
    {
        _context.Livres.Remove(livre);
        return Task.CompletedTask;
    }

    public async Task<bool> BookExistsAsync(int id)
    {
        //AnyAsync() pour vérifier l'existence de manière asynchrone.
        return await _context.Livres.AnyAsync(e => e.Id == id);
    }
}
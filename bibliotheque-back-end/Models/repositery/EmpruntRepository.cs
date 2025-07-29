using bibliotheque_back_end.Data;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque_back_end.Models.repositery;

public class EmpruntRepository: IEmpruntRepository
{
    private readonly BibliothequeDb _context;

    public EmpruntRepository(BibliothequeDb context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Emprunt>> GetAllEmpruntsAsync()
    {
        return await _context.Emprunts
            .Include(e => e.LivresEmpruntes)
            .ThenInclude(el => el.livre)
            .ToListAsync();
    }
 public async Task<Emprunt?> GetEmpruntByIdAsync(int id)
    {
        // Utilise FirstOrDefaultAsync() pour récupérer un emprunt spécifique avec ses livres de manière asynchrone.
        return await _context.Emprunts
                             .Include(e => e.LivresEmpruntes)
                                 .ThenInclude(el => el.livre)
                             .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddEmpruntAsync(Emprunt emprunt)
    {
        // Utilise AddAsync() pour ajouter l'entité de manière asynchrone.
        await _context.Emprunts.AddAsync(emprunt);
    }

    public Task UpdateEmpruntAsync(Emprunt emprunt)
    {
        // Update() est synchrone car il marque simplement l'entité comme modifiée.
        // La persistance réelle se produit avec SaveChangesAsync() (à appeler depuis le service).
        _context.Emprunts.Update(emprunt);
        return Task.CompletedTask; // Retourne un Task complété car pas d'opération asynchrone d'attente ici.
    }

    public Task DeleteEmpruntAsync(Emprunt emprunt)
    {
        // Remove() est synchrone. La persistance se fait via SaveChangesAsync().
        _context.Emprunts.Remove(emprunt);
        return Task.CompletedTask; // Retourne un Task complété.
    }

    public async Task<bool> CheckIfEmpruntExistsAsync(int id)
    {
        // Utilise AnyAsync() pour vérifier l'existence de manière asynchrone.
        return await _context.Emprunts.AnyAsync(e => e.Id == id);
    }

    public async Task<Emprunt?> GetEmpruntWithBooksAsync(int id)
    {
        // Il est préférable de ne pas appeler une méthode asynchrone de manière synchrone, ou de la wrapper.
        // Ici, nous faisons l'appel asynchrone directement pour la cohérence.
        // C'est une duplication du code de GetEmpruntByIdAsync, mais cela respecte la nature asynchrone.
        return await _context.Emprunts
                             .Include(e => e.LivresEmpruntes)
                                 .ThenInclude(el => el.livre) // Vérifiez le nom de la propriété
                             .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Emprunt>> GetEmpruntsContainingBookAsync(int livreId)
    {
        // Utilise ToListAsync() après le filtrage asynchrone.
        return await _context.Emprunts
                             .Include(e => e.LivresEmpruntes)
                                 .ThenInclude(el => el.livre) // Vérifiez le nom de la propriété
                             .Where(e => e.LivresEmpruntes.Any(el => el.IdLivre == livreId))
                             .ToListAsync();
    }
    
    public async Task<IEnumerable<Emprunt>> GetEmpruntsByMembreIdAsync(int membreId)
    {
        // Ceci filtre au niveau de la base de données, ce qui est très performant.
        // Assurez-vous que votre modèle Emprunt a une propriété IdMembre.
        return await _context.Emprunts
            .Include(e => e.LivresEmpruntes) // Incluez les livres si leur état est pertinent
            .Where(e => e.Id == membreId)
            .ToListAsync();
    }
}
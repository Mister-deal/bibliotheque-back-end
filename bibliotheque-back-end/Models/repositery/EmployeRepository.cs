using bibliotheque_back_end.Data;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque_back_end.Models.repositery;

public class EmployeRepository : IEmployeRepository
{
    private readonly BibliothequeDb _context;

    public EmployeRepository(BibliothequeDb context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employe>> GetAllEmployeesAsync()
    {
        // Utilise ToListAsync() pour récupérer toutes les entités de manière asynchrone
        return await _context.Employes.ToListAsync();
    }

    public async Task<Employe?> GetEmployeeAsync(int id)
    {
        // Utilise FindAsync() pour rechercher une entité par clé primaire de manière asynchrone
        // FindAsync gère aussi le suivi des entités par le DbContext
        return await _context.Employes.FindAsync(id);
    }

    public async Task<Employe?> GetEmployeeByEmailAsync(string email)
    {
        // Utilise FirstOrDefaultAsync() pour exécuter la requête de manière asynchrone
        return await _context.Employes.FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());    }

    public async Task AddEmployeeAsync(Employe emp)
    {
        // Utilise AddAsync() pour ajouter une entité de manière asynchrone
        await _context.Employes.AddAsync(emp);
        // Important : Les changements ne sont pas persistés ici.
    }

    public Task UpdateEmployeeAsync(Employe emp)
    {
        // Update() est synchrone car il modifie simplement l'état de l'entité dans le DbContext en mémoire.
        // La persistance réelle se fera lors de l'appel asynchrone à SaveChangesAsync() ailleurs.
        _context.Employes.Update(emp);
        return Task.CompletedTask; // Retourne un Task complété car il n'y a pas d'opération asynchrone d'attente ici.
    }

    public Task DeleteEmployeeAsync(Employe emp)
    {
        // Remove() est synchrone, similaire à Update().
        _context.Employes.Remove(emp);
        return Task.CompletedTask; // Retourne un Task complété.
    }

    public async Task<bool> CheckIfEmployeeExistsAsync(int id)
    {
        // Utilise AnyAsync() pour vérifier l'existence de manière asynchrone
        return await _context.Employes.AnyAsync(e => e.Id == id);
    }
    
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        // Utilise AnyAsync() directement sur le DbSet (IQueryable) pour que la vérification
        // soit effectuée au niveau de la base de données.
        return await _context.Employes.AnyAsync(e => e.Email.ToLower() == email.ToLower());    }
}
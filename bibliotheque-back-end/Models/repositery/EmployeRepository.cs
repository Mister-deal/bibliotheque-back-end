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
        //ToListAsync() pour récupérer toutes les entités de manière asynchrone
        return await _context.Employes.ToListAsync();
    }

    public async Task<Employe?> GetEmployeeAsync(int id)
    {
        return await _context.Employes.FindAsync(id);
    }

    public async Task<Employe?> GetEmployeeByEmailAsync(string email)
    {
        //FirstOrDefaultAsync() pour exécuter la requête de manière asynchrone
        return await _context.Employes.FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());    }

    public async Task AddEmployeeAsync(Employe emp)
    {
        await _context.Employes.AddAsync(emp);
    }

    public Task UpdateEmployeeAsync(Employe emp)
    {
        _context.Employes.Update(emp);
        return Task.CompletedTask; // Retourne un Task complété car il n'y a pas d'opération asynchrone d'attente ici.
    }

    public Task DeleteEmployeeAsync(Employe emp)
    {
        _context.Employes.Remove(emp);
        return Task.CompletedTask; // Retourne un Task complété.
    }

    public async Task<bool> CheckIfEmployeeExistsAsync(int id)
    {
        //AnyAsync() pour vérifier l'existence de manière asynchrone
        return await _context.Employes.AnyAsync(e => e.Id == id);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Employes.AnyAsync(e => e.Email.ToLower() == email.ToLower());
    }
}
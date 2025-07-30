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
        return await _context.Emprunts
                             .Include(e => e.LivresEmpruntes)
                                 .ThenInclude(el => el.livre)
                             .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddEmpruntAsync(Emprunt emprunt)
    {
        await _context.Emprunts.AddAsync(emprunt);
    }

    public Task UpdateEmpruntAsync(Emprunt emprunt)
    {
        _context.Emprunts.Update(emprunt);
        return Task.CompletedTask; 
    }

    public Task DeleteEmpruntAsync(Emprunt emprunt)
    {
        _context.Emprunts.Remove(emprunt);
        return Task.CompletedTask;
    }

    public async Task<bool> CheckIfEmpruntExistsAsync(int id)
    {
        return await _context.Emprunts.AnyAsync(e => e.Id == id);
    }

    public async Task<Emprunt?> GetEmpruntWithBooksAsync(int id)
    {
        return await _context.Emprunts
                             .Include(e => e.LivresEmpruntes)
                                 .ThenInclude(el => el.livre)
                             .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Emprunt>> GetEmpruntsContainingBookAsync(int livreId)
    {
        return await _context.Emprunts
                             .Include(e => e.LivresEmpruntes)
                                 .ThenInclude(el => el.livre) 
                             .Where(e => e.LivresEmpruntes.Any(el => el.IdLivre == livreId))
                             .ToListAsync();
    }
    
    public async Task<IEnumerable<Emprunt>> GetEmpruntsByMembreIdAsync(int membreId)
    {
        return await _context.Emprunts
            .Include(e => e.LivresEmpruntes) 
            .Where(e => e.Id == membreId)
            .ToListAsync();
    }
}
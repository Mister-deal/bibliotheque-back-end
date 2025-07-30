using bibliotheque_back_end.Data;
using Microsoft.EntityFrameworkCore;

namespace bibliotheque_back_end.Models.repositery;

public class MembreRepository : IMembreRepository
{
    private readonly BibliothequeDb _context;

    public MembreRepository(BibliothequeDb context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Membre>> GetAllMembersAsync()
    {
        // Utilise ToListAsync() pour récupérer tous les membres de manière asynchrone.
        return await _context.Membres.ToListAsync();
    }

    public async Task<Membre?> GetMemberAsync(int id)
    {
        // Utilise FindAsync() pour rechercher un membre par clé primaire de manière asynchrone.
        return await _context.Membres.FindAsync(id);
    }

    public async Task<Membre?> GetMemberByEmailAsync(string email)
    {
        // Utilise FirstOrDefaultAsync() pour exécuter la requête de manière asynchrone.
        return await _context.Membres.Where(e => e.Email == email).FirstOrDefaultAsync();
    }

    public async Task AddMemberAsync(Membre member)
    {
        // Utilise AddAsync() pour ajouter l'entité de manière asynchrone.
        await _context.Membres.AddAsync(member);
        // Rappel : les changements devront être sauvegardés via _context.SaveChangesAsync() depuis le service.
    }

    public Task UpdateMemberAsync(Membre member)
    {
        // Update() est synchrone car il modifie l'état de l'entité en mémoire.
        // La persistance réelle se fera lors de l'appel asynchrone à SaveChangesAsync() ailleurs.
        _context.Membres.Update(member);
        return Task.CompletedTask; // Retourne un Task complété.
    }

    public Task DeleteMemberAsync(Membre member)
    {
        // Remove() est synchrone.
        _context.Membres.Remove(member);
        return Task.CompletedTask; // Retourne un Task complété.
    }

    public async Task<bool> CheckIfMemberExistsAsync(int id)
    {
        // Utilise AnyAsync() pour vérifier l'existence de manière asynchrone.
        return await _context.Membres.AnyAsync(e => e.Id == id);
    }
}
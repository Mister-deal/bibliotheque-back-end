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
        //ToListAsync() pour récupérer tous les membres de manière asynchrone.
        return await _context.Membres.ToListAsync();
    }

    public async Task<Membre?> GetMemberAsync(int id)
    {
        //FindAsync() pour rechercher un membre par clé primaire de manière asynchrone.
        return await _context.Membres.FindAsync(id);
    }

    public async Task<Membre?> GetMemberByEmailAsync(string email)
    {
        //FirstOrDefaultAsync() pour exécuter la requête de manière asynchrone.
        return await _context.Membres.Where(e => e.Email == email).FirstOrDefaultAsync();
    }

    public async Task AddMemberAsync(Membre member)
    {
        //AddAsync() pour ajouter l'entité de manière asynchrone.
        await _context.Membres.AddAsync(member);
    }

    public Task UpdateMemberAsync(Membre member)
    {
        _context.Membres.Update(member);
        return Task.CompletedTask; 
    }

    public Task DeleteMemberAsync(Membre member)
    {
        _context.Membres.Remove(member);
        return Task.CompletedTask;
    }

    public async Task<bool> CheckIfMemberExistsAsync(int id)
    {
        //AnyAsync() pour vérifier l'existence de manière asynchrone.
        return await _context.Membres.AnyAsync(e => e.Id == id);
    }
}
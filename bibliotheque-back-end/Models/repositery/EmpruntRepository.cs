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

    public IEnumerable<Emprunt> GetAllEmprunts()
    {
        return _context.Emprunts.Include(e => e.LivresEmpruntes).ThenInclude(el => el.livre).ToList();
    }

    public Emprunt GetEmpruntById(int id)
    {
        return _context.Emprunts.Include(e => e.LivresEmpruntes).ThenInclude(el => el.livre).FirstOrDefault(e => e.Id == id);
    }


    public void AddEmprunt(Emprunt emprunt)
    {
        _context.Emprunts.Add(emprunt);
    }

    public void UpdateEmprunt(Emprunt emprunt)
    {
        _context.Emprunts.Update(emprunt);
    }

    public void DeleteEmprunt(Emprunt emprunt)
    {
        _context.Emprunts.Remove(emprunt);
    }

    public bool CheckIfEmpruntExists(int id)
    {
        return _context.Emprunts.Any(e => e.Id == id);
    }

    public Emprunt GetEmpruntWithBooks(int id)
    {
        return GetEmpruntById(id);
    }

    public IEnumerable<Emprunt> GetEmpruntsContainingBook(int livreId)
    {
        return _context.Emprunts
            .Include(e => e.LivresEmpruntes)
            .ThenInclude(el => el.livre)
            .Where(e => e.LivresEmpruntes.Any(el => el.IdLivre == livreId))
            .ToList();
    }
}
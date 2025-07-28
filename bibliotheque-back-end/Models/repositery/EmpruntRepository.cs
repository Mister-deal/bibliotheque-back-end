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

    public IEnumerable<Emprunt> getAllEmprunts()
    {
        return _context.Emprunts.Include(e => e.LivresEmpruntes).ThenInclude(el => el.livre).ToList();
    }

    public Emprunt getEmpruntById(int id)
    {
        return _context.Emprunts.Include(e => e.LivresEmpruntes).ThenInclude(el => el.livre).FirstOrDefault(e => e.Id == id);
    }


    public void addEmprunt(Emprunt emprunt)
    {
        _context.Emprunts.Add(emprunt);
    }

    public void updateEmprunt(Emprunt emprunt)
    {
        _context.Emprunts.Update(emprunt);
    }

    public void deleteEmprunt(Emprunt emprunt)
    {
        _context.Emprunts.Remove(emprunt);
    }

    public bool checkIfEmpruntExists(int id)
    {
        return _context.Emprunts.Any(e => e.Id == id);
    }

    public Emprunt getEmpruntWithBooks(int id)
    {
        return getEmpruntById(id);
    }

    public IEnumerable<Emprunt> getEmpruntsContainingBook(int livreId)
    {
        return _context.Emprunts
            .Include(e => e.LivresEmpruntes)
            .ThenInclude(el => el.livre)
            .Where(e => e.LivresEmpruntes.Any(el => el.IdLivre == livreId))
            .ToList();
    }
}
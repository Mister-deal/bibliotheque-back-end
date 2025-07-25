using bibliotheque_back_end.Data;

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
        throw new NotImplementedException();
    }

    public Emprunt getEmpruntByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public void addEmprunt(Emprunt emprunt)
    {
        throw new NotImplementedException();
    }

    public void updateEmprunt(Emprunt emprunt)
    {
        throw new NotImplementedException();
    }

    public void deleteEmprunt(Emprunt emprunt)
    {
        throw new NotImplementedException();
    }

    public bool checkIfEmpruntExists(string email)
    {
        throw new NotImplementedException();
    }
}
using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class EmpruntService: IEmpruntService
{
    private readonly IEmpruntRepository _empruntRepository;
    private readonly ILivreRepository _livreRepository;
    private readonly IMembreRepository _membreRepository;

    public EmpruntService(IEmpruntRepository empruntRepository, ILivreRepository livreRepository, IMembreRepository membreRepository)
    {
        _empruntRepository = empruntRepository;
        _livreRepository = livreRepository;
        _membreRepository = membreRepository;
    }

    public IEnumerable<Emprunt> GetAllEmprunts()
    {
        return _empruntRepository.GetAllEmprunts();
    }

    public IEnumerable<Emprunt> GetEmpruntsByMembreId(int membreId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Emprunt> GetEmpruntsByLivreId(int livreId)
    {
        throw new NotImplementedException();
    }

    public Emprunt GetEmpruntById(int id)
    {
        if(id <= 0) throw new ArgumentException("EmpruntId doit être supérieur à 0");
        var emprunt = _empruntRepository.GetEmpruntById(id);
        return emprunt;
    }

    public IEnumerable<Emprunt> GetActiveEmprunts()
    {
        throw new NotImplementedException();
    }

    public Emprunt CreateEmprunt(int membreId, List<int> livreIds, DateOnly dateRetour, int employeValidationId)
    {
        throw new NotImplementedException();
    }

    public Emprunt ReturnSpecificBookFromEmprunt(int empruntId, int livreId, int employeValidationId)
    {
        throw new NotImplementedException();
    }

    public Emprunt DeleteEmprunt(int id)
    {
        throw new NotImplementedException();
    }

    public bool EmpruntExists(int id)
    {
        return _empruntRepository.CheckIfEmpruntExists(id);
    }
}
using bibliotheque_back_end.Data;

namespace bibliotheque_back_end.Models.repositery;

public class MembreRepository: IMembreRepository
{
    private readonly BibliothequeDb _context;

    public MembreRepository(BibliothequeDb context)
    {
        _context = context;
    }

    public IEnumerable<Membre> getAllMembers()
    {
        throw new NotImplementedException();
    }

    public Membre getMember(int id)
    {
        throw new NotImplementedException();
    }

    public Membre getMemberByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public void AddMember(Membre member)
    {
        throw new NotImplementedException();
    }

    public void UpdateMember(Membre member)
    {
        throw new NotImplementedException();
    }

    public void DeleteMember(Membre member)
    {
        throw new NotImplementedException();
    }

    public bool CheckIfMemberExists(int id)
    {
        throw new NotImplementedException();
    }
}
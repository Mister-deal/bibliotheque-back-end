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
        return _context.Membres.ToList();
    }

    public Membre getMember(int id)
    {
       return _context.Membres.Find(id);
    }

    public Membre getMemberByEmail(string email)
    {
        return _context.Membres.Where(e => e.Email == email).FirstOrDefault();
    }

    public void AddMember(Membre member)
    {
        _context.Membres.Add( member);
    }

    public void UpdateMember(Membre member)
    {
        _context.Membres.Update(member);
    }

    public void DeleteMember(Membre member)
    {
        _context.Membres.Remove(member);
    }

    public bool CheckIfMemberExists(int id)
    {
        return _context.Membres.Any(e => e.Id == id);
    }
}
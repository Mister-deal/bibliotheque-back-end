namespace bibliotheque_back_end.Models.repositery;

public interface IMembreRepository
{
    IEnumerable<Membre> getAllMembers();
    Membre getMember(int id);
    Membre getMemberByEmail(string email);
    void AddMember(Membre member);
    void UpdateMember(Membre member);
    void DeleteMember(Membre member);
    bool CheckIfMemberExists(int id);
}
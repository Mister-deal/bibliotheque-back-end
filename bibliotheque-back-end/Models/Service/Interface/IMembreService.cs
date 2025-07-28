namespace bibliotheque_back_end.Models.Service.Interface;

public interface IMembreService
{
    IEnumerable<Membre> GetAllMembers();
    Membre GetMemberById(int id);
    Membre GetMemberByEmail(string email);
    
    Membre AddMember(Membre newMember, string dataPassword);
    
    Membre UpdateMember(int id, Membre updatedMember);
    Membre UpdatePasswordMember(int id, string oldPassword, string newPassword);

    Membre DeleteMember(int id);
    
    bool MemberExists(int id);
}
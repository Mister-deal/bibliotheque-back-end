namespace bibliotheque_back_end.Models.Service.Interface;

public interface IMembreService
{
    IEnumerable<Membre> GetAllMembers();
    Employe GetMemberById(int id);
    Employe GetMemberByEmail(string email);
    
    Employe AddMember(Membre newMember, string dataPassword);
    
    Employe UpdateMember(int id, Membre updatedMember);

    Employe DeleteMember(int id);
    
    bool employeeExists(int id);
}
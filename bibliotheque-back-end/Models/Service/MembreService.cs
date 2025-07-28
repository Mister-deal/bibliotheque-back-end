using bibliotheque_back_end.Models.Service.Interface;

namespace bibliotheque_back_end.Models.Service;

public class MembreService: IMembreService
{
    public IEnumerable<Membre> GetAllMembers()
    {
        throw new NotImplementedException();
    }

    public Employe GetMemberById(int id)
    {
        throw new NotImplementedException();
    }

    public Employe GetMemberByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Employe AddMember(Membre newMember, string dataPassword)
    {
        throw new NotImplementedException();
    }

    public Employe UpdateMember(int id, Membre updatedMember)
    {
        throw new NotImplementedException();
    }

    public Employe DeleteMember(int id)
    {
        throw new NotImplementedException();
    }

    public bool employeeExists(int id)
    {
        throw new NotImplementedException();
    }
}
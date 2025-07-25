using bibliotheque_back_end.Data;

namespace bibliotheque_back_end.Models.repositery;

public class EmployeRepository: IEmployeRepository
{
    private readonly BibliothequeDb _context;

    public EmployeRepository(BibliothequeDb context)
    {
        _context = context;
    }

    public IEnumerable<Employe> getAllEmployees()
    {
        throw new NotImplementedException();
    }

    public Employe getEmployee(int id)
    {
        throw new NotImplementedException();
    }

    public Employe getEmployeeByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public void AddEmployee(Employe emp)
    {
        throw new NotImplementedException();
    }

    public void UpdateEmployee(Employe emp)
    {
        throw new NotImplementedException();
    }

    public void DeleteEmployee(Employe emp)
    {
        throw new NotImplementedException();
    }

    public bool EmployeeExists(int id)
    {
        throw new NotImplementedException();
    }
}
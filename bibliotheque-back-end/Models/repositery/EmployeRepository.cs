using bibliotheque_back_end.Data;

namespace bibliotheque_back_end.Models.repositery;

public class EmployeRepository: IEmployeRepository
{
    private readonly BibliothequeDb _context;

    public EmployeRepository(BibliothequeDb context)
    {
        _context = context;
    }

    public IEnumerable<Employe> GetAllEmployees()
    {
        return _context.Employes.ToList();
    }

    public Employe GetEmployee(int id)
    {
        return _context.Employes.Find(id);
    }

    public Employe GetEmployeeByEmail(string email)
    {
        return _context.Employes.Where(e => e.Email == email).FirstOrDefault();
    }

    public void AddEmployee(Employe emp)
    {
        _context.Employes.Add(emp);
    }

    public void UpdateEmployee(Employe emp)
    {
        _context.Employes.Update(emp);
    }

    public void DeleteEmployee(Employe emp)
    {
        _context.Employes.Remove(emp);
    }

    public bool CheckIfEmployeeExists(int id)
    {
        return _context.Employes.Any(e => e.Id == id);
    }
}
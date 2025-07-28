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
        return _context.Employes.ToList();
    }

    public Employe getEmployee(int id)
    {
        return _context.Employes.Find(id);
    }

    public Employe getEmployeeByEmail(string email)
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

    public bool EmployeeExists(int id)
    {
        return _context.Employes.Any(e => e.Id == id);
    }
}
namespace bibliotheque_back_end.Models.repositery;

public interface IEmployeRepository
{
    IEnumerable<Employe> getAllEmployees();
    Employe getEmployee(int id);
    Employe getEmployeeByEmail(string email);
    void AddEmployee(Employe emp);
    void UpdateEmployee(Employe emp);
    void DeleteEmployee(Employe emp);
    bool EmployeeExists(int id);
}
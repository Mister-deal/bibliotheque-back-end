namespace bibliotheque_back_end.Models.repositery;

public interface IEmployeRepository
{
    IEnumerable<Employe> GetAllEmployees();
    Employe GetEmployee(int id);
    Employe GetEmployeeByEmail(string email);
    void AddEmployee(Employe emp);
    void UpdateEmployee(Employe emp);
    void DeleteEmployee(Employe emp);
    bool CheckIfEmployeeExists(int id);
}